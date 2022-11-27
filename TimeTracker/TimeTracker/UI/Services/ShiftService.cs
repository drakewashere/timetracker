using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using UI.Data;
using UI.Data.DTOs;
using UI.Services.Interfaces;

namespace UI.Services
{
    public class ShiftService : IShiftService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ShiftService> _logger;
        private DbSet<Shift> Shifts { get { return _context.Shifts; } }

        public ShiftService(
            ApplicationDbContext context,
            ILogger<ShiftService> logger
        )
        {
            _context = context;
            _logger = logger;
        }

        private static Expression<Func<Shift, bool>> IsActive
            => s => s.DeletedDate == null;

        public async Task<Shift?> GetShiftById(long shiftId)
            => await Shifts
                .Where(IsActive)
                .FirstOrDefaultAsync(s => s.ShiftId == shiftId);

        public async Task<IEnumerable<Shift>> GetShiftsForUserAudit(long userId)
            => await Task.Run(() => Shifts
            .Where(s => s.UserId == userId));

        public async Task<IEnumerable<Shift>> GetShiftsForUser(long userId)
            => await Task.Run(() => Shifts
            .Where(IsActive)
            .Where(s => s.UserId == userId));

        public async Task<Shift?> GetOpenShiftForUser(long userId)
            => await Shifts
                .Where(IsActive)
                .Where(s => s.UserId == userId && s.EndTime == null)
                .OrderBy(b => b.StartTime)
                .FirstOrDefaultAsync();

        public async Task<Shift> EndCurrentShiftForUser(long userId)
        {
            var currentShift = await GetOpenShiftForUser(userId);

            if (currentShift == null)
            {
                _logger.LogWarning($"Unable to find an open shift to close for user {userId}");
                throw new Exception($"Unable to find an open shift to close");
            }

            currentShift.EndTime = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return currentShift;
        }

        public async Task<Shift> CreateShiftForUser(long userId)
        {
            var openShifts = await GetShiftsForUser(userId);
            if (openShifts.Any(b => b.EndTime == null))
            {
                _logger.LogWarning($"Cannot start shift for user {userId}, as another shift(s) is currently open");
                throw new Exception($"Cannot start a new shift with an open shift");
            }

            var newShift = new Shift
            {
                UserId = userId,
                StartTime = DateTime.UtcNow
            };
            await Shifts.AddAsync(newShift);
            await _context.SaveChangesAsync();

            return newShift;
        }

        public async Task<Shift> AddInsertShift(Shift currentShift)
        {
            if (currentShift.EditedByUser == null)
                throw new Exception("Cannot edit shift with no editing user");

            currentShift.EditedDate = DateTime.UtcNow;

            if (currentShift.ShiftId == 0)
                await Shifts.AddAsync(currentShift);
            else
                Shifts.Update(currentShift);

            await _context.SaveChangesAsync();

            return currentShift;
        }

        public async Task<Shift> DeleteShift(long ShiftId, long userId)
        {
            var currentShift = await GetShiftById(ShiftId);
            if (currentShift == null)
            {
                throw new Exception("Cannot delete shift that does not exist");
            }
            currentShift.DeletedDate = DateTime.UtcNow;
            currentShift.DeletedByUser = userId;

            await _context.SaveChangesAsync();

            return currentShift;
        }
    }
}
