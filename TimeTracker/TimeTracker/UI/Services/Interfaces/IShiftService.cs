using System.Drawing;
using UI.Data.DTOs;

namespace UI.Services.Interfaces
{
    public interface IShiftService
    {
        Task<Shift> AddInsertShift(Shift currentShift);
        Task<Shift> CreateShiftForUser(string userId);
        Task<Shift> DeleteShift(long ShiftId, string userId);
        Task<Shift> EndCurrentShiftForUser(string userId);
        Task<IEnumerable<Report>> GenerateReportSource(string? userId = null, DateTimeOffset? StartDate = null, DateTimeOffset? EndDate = null);
        Task<Shift?> GetOpenShiftForUser(string userId);
        Task<Shift?> GetShiftById(long shiftId);
        Task<IEnumerable<Shift>> GetShiftsForUser(string userId);
        Task<IEnumerable<Shift>> GetShiftsForUserAudit(string userId);
    }
}
