using BetsApi.Models;
using BetsApi.Repositories;
using Microsoft.EntityFrameworkCore;

namespace BetsApi.Data.Repositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(BetsDbContext context) : base(context)
        {
        }
        public async Task<User> GetByNameAsync(string name)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Name == name);
        }

        public async Task<IEnumerable<User>> GetUsersWithBetsAsync()
        {
            return await _context.Users.Include(u => u.BetsSent).Include(u => u.BetsReceived).ToListAsync();
        }
    }
}
