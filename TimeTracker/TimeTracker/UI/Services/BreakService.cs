using Microsoft.EntityFrameworkCore;
using UI.Data.DTOs;
using UI.Data;
using System.Linq.Expressions;
using UI.Services.Interfaces;
using UI.Pages.TimeTracking;

namespace UI.Services
{
    public class BreakService : IBreakService
    {
        private readonly ILogger<BreakService> _logger;
        private readonly IShiftService _shift;
        private readonly ApplicationDbContext _context;
        private DbSet<Break> Breaks { get { return _context.Breaks; } }

        public BreakService(
            ApplicationDbContext context,
            IShiftService shiftService,
            ILogger<BreakService> logger
        )
        {
            _context = context;
            _shift = shiftService;
            _logger = logger;

            SeedBreakTypes(_context);
        }

        private static bool BreakTypesSeeded { get; set; }

        private static void SeedBreakTypes(ApplicationDbContext context)
        {
            if (BreakTypesSeeded)
                return;

            foreach(var breakType in Enum.GetValues(typeof(BreakTypeId)))
            {
                var instance = context.BreakTypes
                    .FirstOrDefault(bt => bt.BreakTypeId == (BreakTypeId)breakType);
                if (instance == null)
                {
                    context.Add(new BreakType { BreakTypeId = (BreakTypeId)breakType, Name = ((BreakTypeId)breakType).ToString() });
                    context.SaveChanges();
                }
            }
        }

        private static Expression<Func<Break, bool>> IsActive
            => b => b.DeletedDate == null && b.Shift.DeletedDate == null;

        public async Task<Break?> GetBreakById(long breakId)
            => await Breaks
            .Where(IsActive)
            .FirstOrDefaultAsync(b => b.BreakId == breakId);

        public async Task<IEnumerable<Break>> GetBreaksForUserAudit(string userId)
            => await Task.Run(() => Breaks
            .Where(b => b.Shift.UserId == userId));

        public async Task<IEnumerable<Break>> GetBreaksForUser(string userId)
            => await Task.Run(() => Breaks
            .Where(IsActive)
            .Where(b => b.Shift.UserId == userId));

        public async Task<IEnumerable<Break>> GetBreaksForShiftAudit(long shiftId)
            => await Task.Run(() => Breaks
            .Where(b => b.ShiftId == shiftId));

        public async Task<IEnumerable<Break>> GetBreaksForShift(long shiftId)
            => await Task.Run(() => Breaks
            .Where(IsActive)
            .Where(b => b.ShiftId == shiftId));

        public async Task<Break?> GetOpenBreakForUser(string userId)
            => await Breaks
                .Where(IsActive)
                .Where(b => b.Shift.UserId == userId && b.EndTime == null && b.StartTime < DateTime.UtcNow)
                .OrderBy(b => b.StartTime)
                .FirstOrDefaultAsync();

        public async Task<Break> EndCurrentBreakForUser(string userId)
        {
            var currentBreak = await GetOpenBreakForUser(userId);

            if (currentBreak == null)
            {
                _logger.LogWarning($"Unable to find an open break to close for user {userId}");
                throw new Exception($"Unable to find an open break to close");
            }

            currentBreak.EndTime = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return currentBreak;
        }

        public async Task<Break> CreateBreakForUser(string userId, BreakTypeId breakType)
        {
            var openShift = await _shift.GetOpenShiftForUser(userId);
            if (openShift == null)
            {
                _logger.LogWarning($"Cannot start break for user {userId}, as no shift is currently open");
                throw new Exception($"Cannot start a new break with no open shift");
            }

            var openBreaks = await GetBreaksForShift(openShift.ShiftId);
            if (openBreaks.Any(b => b.EndTime == null))
            {
                _logger.LogWarning($"Cannot start break for user {userId}, as another break(s) is currently open");
                throw new Exception($"Cannot start a new break with an open break");
            }

            var newBreak = new Break
            {
                ShiftId = openShift.ShiftId,
                StartTime = DateTime.UtcNow,
                BreakTypeId = breakType
            };
            await Breaks.AddAsync(newBreak);
            await _context.SaveChangesAsync();

            return newBreak;
        }

        public async Task<Break> AddInsertBreak(Break currentBreak)
        {
            if (await _shift.GetShiftById(currentBreak.ShiftId) == null)
            {
                _logger.LogWarning($"Cannot add break for shift {currentBreak.ShiftId}, shift not found");
                throw new Exception("Cannot add break for shift that does not exist");
            }

            if (currentBreak.EditedByUser == null)
                throw new Exception("Cannot edit break with no editing user");

            if (currentBreak.StartTime == default)
                currentBreak.StartTime = DateTime.UtcNow;

            if (currentBreak.EndTime != null && currentBreak.EndTime < currentBreak.StartTime)
                throw new Exception("Cannot set a end time before the start time");

            currentBreak.EditedDate = DateTime.UtcNow;

            if (currentBreak.BreakId == 0)
                await Breaks.AddAsync(currentBreak);
            else
                Breaks.Update(currentBreak);

            await _context.SaveChangesAsync();

            return currentBreak;
        }

        public async Task<Break> DeleteBreak(long breakId, string userId)
        {
            var currentBreak = await GetBreakById(breakId);
            if (currentBreak == null)
            {
                throw new Exception("Cannot delete break that does not exist");
            }
            currentBreak.DeletedDate = DateTime.UtcNow;
            currentBreak.DeletedByUser = userId;

            await _context.SaveChangesAsync();

            return currentBreak;
        }
    }
}
