using Microsoft.EntityFrameworkCore;
using UI.Data;
using UI.Data.DTOs;
using UI.Services.Interfaces;

namespace UI.Services
{
    public class ShiftService : IShiftService
    {
        private DbSet<Shift> Shifts { get; set; }

        public ShiftService(ApplicationDbContext context) 
        { 
            Shifts = context.Shifts;
        }

        public async Task<Shift?> GetShiftById(long shiftId)
            => throw new NotImplementedException();

        public Task<Shift?> GetOpenShift(long userId)
            => throw new NotImplementedException();
    }
}
