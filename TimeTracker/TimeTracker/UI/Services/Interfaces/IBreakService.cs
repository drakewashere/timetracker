using UI.Data.DTOs;

namespace UI.Services.Interfaces
{
    public interface IBreakService
    {
        Task<Break> AddInsertBreak(Break currentBreak);
        Task<Break> CreateBreakForUser(string userId, BreakTypeId breakTypeName);
        Task<Break> DeleteBreak(long breakId, string userId);
        Task<Break> EndCurrentBreakForUser(string userId);
        Task<Break?> GetBreakById(long breakId);
        Task<IEnumerable<Break>> GetBreaksForShift(long shiftId);
        Task<IEnumerable<Break>> GetBreaksForShiftAudit(long shiftId);
        Task<IEnumerable<Break>> GetBreaksForUser(string userId);
        Task<IEnumerable<Break>> GetBreaksForUserAudit(string userId);
        Task<Break?> GetOpenBreakForUser(string userId);
    }
}
