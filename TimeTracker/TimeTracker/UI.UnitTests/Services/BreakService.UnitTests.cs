using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UI.Data.DTOs;
using UI.Services.Interfaces;
using UI.Services;
using Microsoft.Extensions.Logging;

namespace UI.UnitTests.Services
{
    public class BreakServiceUnitTests
    {
        private ILoggerFactory? LoggerFactory = new ServiceCollection()
            .AddLogging()
            .BuildServiceProvider()
            .GetService<ILoggerFactory>();

        private IBreakService CreateBreakService(IQueryable<Shift>? shifts = null, IQueryable<Break>? breaks = null)
        {
            var context = new MoqDbContext(shifts, breaks).GetDbContext();
            var shiftLogger = LoggerFactory!.CreateLogger<ShiftService>();
            var breakLogger = LoggerFactory!.CreateLogger<BreakService>();

            var shiftService = new ShiftService(context, shiftLogger);

            return new BreakService(context,shiftService, breakLogger);
        }

        [Fact]
        public async Task CanGetBreakById()
        {
            var breaks = new List<Break>
            {
                new Break { BreakId = 12, ShiftId = 1, Shift = new Shift {ShiftId = 1 } },
            }.AsQueryable();
            var _breaks = CreateBreakService(breaks: breaks);

            var userBreak = await _breaks.GetBreakById(12);
            Assert.NotNull(userBreak);
            Assert.Equal("12", userBreak.BreakId.ToString());
        }

        [Fact]
        public async Task CanGetBreaksForUser()
        {
            var breaks = new List<Break>
            {
                new Break { BreakId = 12, Shift = new Shift {ShiftId = 1, UserId = "1" } },
            }.AsQueryable();
            var _breaks = CreateBreakService(breaks: breaks);

            var userBreak = await _breaks.GetBreaksForUser("1");
            Assert.NotEmpty(userBreak);
            Assert.Equal("12", userBreak.First().BreakId.ToString());
        }

        [Fact]
        public async Task CanGetBreaksForShift()
        {
            var breaks = new List<Break>
            {
                new Break { BreakId = 12, ShiftId = 10, Shift = new Shift() },
            }.AsQueryable();
            var _breaks = CreateBreakService(breaks: breaks);

            var userBreak = await _breaks.GetBreaksForShift(10);
            Assert.NotEmpty(userBreak);
            Assert.Equal("10", userBreak.First().ShiftId.ToString());
        }

        [Fact]
        public async Task CanGetOpenBreakForUser()
        {
            var breaks = new List<Break>
            {
                new Break { BreakId = 10, ShiftId = 10, Shift = new Shift { UserId = "15" }, EndTime = DateTime.UtcNow.AddMinutes(-5) },
                new Break { BreakId = 12, ShiftId = 10, Shift = new Shift { UserId = "15" } },
            }.AsQueryable();
            var _breaks = CreateBreakService(breaks: breaks);

            var userBreak = await _breaks.GetOpenBreakForUser("15");
            Assert.NotNull(userBreak);
            Assert.Equal("12", userBreak.BreakId.ToString());
            Assert.Null(userBreak.EndTime);
        }

        [Fact]
        public async Task CanEndOpenBreakForUser()
        {
            var breaks = new List<Break>
            {
                new Break { BreakId = 10, ShiftId = 10, Shift = new Shift { UserId = "15" }, EndTime = DateTime.UtcNow.AddMinutes(-5) },
                new Break { BreakId = 12, ShiftId = 10, Shift = new Shift { UserId = "15" } },
            }.AsQueryable();
            var _breaks = CreateBreakService(breaks: breaks);

            var userBreak = await _breaks.EndCurrentBreakForUser("15");
            Assert.NotNull(userBreak);
            Assert.Equal("12", userBreak.BreakId.ToString());
            Assert.NotNull(userBreak.EndTime);
        }

        [Fact]
        public async Task CanCreateNewBreak()
        {
            var shifts = new List<Shift>
            {
                new Shift { UserId = "15"}
            }.AsQueryable();
            var _breaks = CreateBreakService(shifts: shifts);

            var userBreak = await _breaks.CreateBreakForUser("15", BreakTypeId.Break);

            Assert.NotNull(userBreak);
        }

        [Fact]
        public async Task CantCreateNewBreak_NoShift()
        {
            var _breaks = CreateBreakService();

            await Assert.ThrowsAsync<Exception>(async () => await _breaks.CreateBreakForUser("15", BreakTypeId.Break));
        }

        [Fact]
        public async Task CantCreateNewBreak_OpenBreak()
        {
            var breaks = new List<Break>
            {
                new Break { ShiftId = 1, Shift = new Shift {ShiftId = 1, UserId = "15" } }
            }.AsQueryable();
            var shifts = new List<Shift>
            {
                new Shift { ShiftId = 1, UserId = "15"}
            }.AsQueryable();
            var _breaks = CreateBreakService(shifts, breaks);

            await Assert.ThrowsAsync<Exception>(async () => await _breaks.CreateBreakForUser("15", BreakTypeId.Break));
        }
    }
}
