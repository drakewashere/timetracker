using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UI.Data;
using UI.Services.Interfaces;

namespace UI.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;
        private DbSet<IdentityUser> Users { get { return _context.Users; } }
        public UserService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Tuple<string, string>>> GetUserList()
        {
            var a = await Task.Run(() => Users.Select(u => new Tuple<string, string>(u.Id, u.UserName)));
            return a;
        }
    }
}
