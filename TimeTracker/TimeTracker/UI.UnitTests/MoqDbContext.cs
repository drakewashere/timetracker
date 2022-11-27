using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using MockQueryable.Moq;
using Moq;
using Newtonsoft.Json.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Sockets;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using UI.Data;
using UI.Data.DTOs;

namespace UI.UnitTests
{
    internal class MoqDbContext
    {
        public MoqDbContext()
        {
            Shifts = Enumerable.Empty<Shift>().AsQueryable().BuildMockDbSet();
            Breaks = Enumerable.Empty<Break>().AsQueryable().BuildMockDbSet();
        }
        public MoqDbContext(IQueryable<Shift>? shifts = null, IQueryable<Break>? breaks = null)
        {
            Shifts = (shifts ?? Enumerable.Empty<Shift>().AsQueryable()).BuildMockDbSet();
            Breaks = (breaks ?? Enumerable.Empty<Break>().AsQueryable()).BuildMockDbSet();
        }
        private readonly Mock<DbSet<Shift>> Shifts = new();
        private readonly Mock<DbSet<Break>> Breaks = new();

        public ApplicationDbContext GetDbContext()
        {
            var mockContext = new Mock<ApplicationDbContext>(new DbContextOptions<ApplicationDbContext>());
            mockContext.Setup(c => c.Shifts).Returns(Shifts.Object);
            mockContext.Setup(c => c.Breaks).Returns(Breaks.Object);

            return mockContext.Object;
        }
    }
}