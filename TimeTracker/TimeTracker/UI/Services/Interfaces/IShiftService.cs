using System.Drawing;
using UI.Data.DTOs;

namespace UI.Services.Interfaces
{
    public interface IShiftService
    {
        Task<Shift> AddInsertShift(Shift currentShift);
        Task<Shift> CreateShiftForUser(long userId);
        Task<Shift> DeleteShift(long ShiftId, long userId);
        Task<Shift> EndCurrentShiftForUser(long userId);
        Task<Shift?> GetOpenShiftForUser(long userId);
        Task<Shift?> GetShiftById(long shiftId);
        Task<IEnumerable<Shift>> GetShiftsForUser(long userId);
        Task<IEnumerable<Shift>> GetShiftsForUserAudit(long userId);
    }
}
