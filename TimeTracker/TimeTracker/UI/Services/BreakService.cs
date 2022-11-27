using Microsoft.EntityFrameworkCore;
using UI.Data.DTOs;
using UI.Data;
using System.Linq.Expressions;
using UI.Services.Interfaces;

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
        }

        private static Expression<Func<Break, bool>> IsActive
            => b => b.DeletedDate == null && b.Shift.DeletedDate == null;

        public async Task<Break?> GetBreakById(long breakId)
            => await Breaks
            .Where(IsActive)
            .FirstOrDefaultAsync(b => b.BreakId == breakId);

        public async Task<IEnumerable<Break>> GetBreaksForUserAudit(long userId)
            => await Task.Run(() => Breaks
            .Where(b => b.Shift.UserId == userId));

        public async Task<IEnumerable<Break>> GetBreaksForUser(long userId)
            => await Task.Run(() => Breaks
            .Where(IsActive)
            .Where(b => b.Shift.UserId == userId));

        public async Task<IEnumerable<Break>> GetBreaksForShiftAudit(long shiftId)
            => await Task.Run(() => Breaks
            .Where(b => b.ShiftId == shiftId));

        public async Task<IEnumerable<Break>> GetBreaksForShift(long shiftId)
            => await Task.Run( () => Breaks
            .Where(IsActive)
            .Where(b => b.ShiftId == shiftId));

        public async Task<Break?> GetOpenBreak(long userId)
            => await Breaks
                .Where(IsActive)
                .Where(b => b.Shift.UserId == userId)
                .OrderBy(b => b.StartTime)
                .FirstOrDefaultAsync();

        public async Task<Break> EndCurrentBreak(long userId)
        {
            var currentBreak = await GetOpenBreak(userId);

            if (currentBreak == null)
            {
                _logger.LogWarning($"Unable to find an open break to close for user {userId}");
                throw new Exception($"Unable to find an open break to close");
            }

            currentBreak.EndTime = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return currentBreak;
        }

        public async Task<Break> CreateBreakForUser(long userId, int breakTypeId)
        {
            var openBreak = await GetOpenBreak(userId);
            if (openBreak != null)
            {
                _logger.LogWarning($"Cannot start break for user {userId}, as break {openBreak.BreakId} is currently open");
                throw new Exception($"Cannot start a new break with a break open");
            }

            var openShift = await _shift.GetOpenShift(userId);
            if (openShift == null) 
            {
                _logger.LogWarning($"Cannot start break for user {userId}, as no shift is currently open");
                throw new Exception($"Cannot start a new break with no open shift");
            }

            var newBreak = new Break
            {
                ShiftId = openShift.ShiftId,
                StartTime = DateTime.UtcNow,
                BreakTypeId = breakTypeId
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

            if (currentBreak.BreakId == 0)
                await Breaks.AddAsync(currentBreak);
            else
            {
                if (currentBreak.EditedByUser == null)
                    throw new Exception("Cannot edit break with no editing user");

                currentBreak.EditedDate = DateTime.UtcNow;
                Breaks.Update(currentBreak);
            }
            
            await _context.SaveChangesAsync();

            return currentBreak;
        }

        public async Task<Break> DeleteBreak(long breakId, long userId)
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
