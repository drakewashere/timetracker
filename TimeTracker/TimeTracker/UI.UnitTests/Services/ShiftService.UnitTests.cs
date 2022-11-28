using Microsoft.Build.Framework;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UI.Data.DTOs;
using UI.Services;
using UI.Services.Interfaces;

namespace UI.UnitTests.Services
{
    public class ShiftServiceUnitTests
    {
        private ILoggerFactory? LoggerFactory = new ServiceCollection()
            .AddLogging()
            .BuildServiceProvider()
            .GetService<ILoggerFactory>();

        private IShiftService CreateShiftService(IQueryable<Shift>? shifts = null)
        {
            var context = new MoqDbContext(shifts).GetDbContext();
            var logger = LoggerFactory!.CreateLogger<ShiftService>();

            return new ShiftService(context, logger);
        }

        [Fact]
        public async Task CanCreateShift()
        {
            var _shift = CreateShiftService();

            var newShift = await _shift.CreateShiftForUser("1");
            Assert.NotNull(newShift);
        }

        [Fact]
        public async Task CantCreateShiftWithShiftOpen()
        {
            var shifts = new List<Shift>
            {
                new Shift {UserId = "1"},
            }.AsQueryable();
            var _shift = CreateShiftService(shifts);

            await Assert.ThrowsAsync<Exception>(async () => await _shift.CreateShiftForUser("1"));
        }

        [Fact]
        public async Task CanGetShiftForUser()
        {
            var shifts = new List<Shift>
            {
                new Shift {UserId = "1"},
            }.AsQueryable();
            var _shift = CreateShiftService(shifts);

            var userShifts = await _shift.GetShiftsForUser("1");
            Assert.NotEmpty(userShifts);
        }

        [Fact]
        public async Task CanGetShiftById()
        {
            var shifts = new List<Shift>
            {
                new Shift {ShiftId = 12, UserId = "1", EndTime = DateTime.UtcNow.AddMinutes(-10)}
            }.AsQueryable();
            var _shift = CreateShiftService(shifts);

            var userShift = await _shift.GetShiftById(12);
            Assert.NotNull(userShift);
        }

        [Fact]
        public async Task CanGetOpenShiftForUser()
        {
            var shifts = new List<Shift>
            {
                new Shift {UserId = "1", EndTime = DateTime.UtcNow.AddMinutes(-10)},
                new Shift {UserId = "1"}
            }.AsQueryable();
            var _shift = CreateShiftService(shifts);

            var userShift = await _shift.GetOpenShiftForUser("1");
            Assert.NotNull(userShift);
            Assert.Null(userShift.EndTime);
        }

        [Fact]
        public async Task CanEndCurrentShift()
        {
            var shifts = new List<Shift>
            {
                new Shift {UserId = "1", EndTime = DateTime.UtcNow.AddMinutes(-10)},
                new Shift {ShiftId = 12, UserId = "1"}
            }.AsQueryable();
            var _shift = CreateShiftService(shifts);

            var userShift = await _shift.EndCurrentShiftForUser("1");
            Assert.NotNull(userShift);
            Assert.NotNull(userShift.EndTime);
            Assert.Equal("12", userShift.ShiftId.ToString());
        }

        [Fact]
        public async Task CantEndShiftNoShift()
        {
            var _shift = CreateShiftService();

            await Assert.ThrowsAsync<Exception>(async () => await _shift.EndCurrentShiftForUser("1"));
        }
    }
}
