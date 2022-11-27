using System.Drawing;
using UI.Data.DTOs;

namespace UI.Services.Interfaces
{
    public interface IShiftService
    {
        Task<Shift> AddInsertShift(Shift currentShift);
        Task<Shift> CreateShiftForUser(long userId, int ShiftTypeId);
        Task<Shift> DeleteShift(long ShiftId, long userId);
        Task<Shift> EndCurrentShift(long userId);
        Task<Shift?> GetOpenShiftForUser(long userId);
        Task<Shift?> GetShiftById(long shiftId);
        Task<IEnumerable<Shift>> GetShiftsForUser(long userId);
        Task<IEnumerable<Shift>> GetShiftsForUserAudit(long userId);
    }
}
