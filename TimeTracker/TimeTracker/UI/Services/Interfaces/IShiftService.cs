using System.Drawing;
using UI.Data.DTOs;

namespace UI.Services.Interfaces
{
    public interface IShiftService
    {
        Task<Shift?> GetShiftById(long shiftId);
        Task<Shift?> GetOpenShift(long userId);
    }
}
