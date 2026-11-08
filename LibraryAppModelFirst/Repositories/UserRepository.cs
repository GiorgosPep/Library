using LibraryAppModelFirst.Data;
using LibraryAppModelFirst.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryAppModelFirst.Repositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(LibDbContext context) : base(context)
        {
            
        }

        public async Task<User?> FindByUserName(string username)
        {
            return await _context.Users
            .Include(u => u.Role)           // Eager Loading - Join
                .ThenInclude(r => r.Capabilities)
            .FirstOrDefaultAsync(u => u.Username == username || u.Email == username);
        }
    }
}
