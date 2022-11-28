using UI.Data.DTOs;

namespace UI.Services.Interfaces
{
    public interface IBreakService
    {
        Task<Break> AddInsertBreak(Break currentBreak);
        Task<Break> CreateBreakForUser(long userId, int breakTypeId);
        Task<Break> DeleteBreak(long breakId, long userId);
        Task<Break> EndCurrentBreakForUser(long userId);
        Task<Break?> GetBreakById(long breakId);
        Task<IEnumerable<Break>> GetBreaksForShift(long shiftId);
        Task<IEnumerable<Break>> GetBreaksForShiftAudit(long shiftId);
        Task<IEnumerable<Break>> GetBreaksForUser(long userId);
        Task<IEnumerable<Break>> GetBreaksForUserAudit(long userId);
        Task<Break?> GetOpenBreakForUser(long userId);
    }
}
