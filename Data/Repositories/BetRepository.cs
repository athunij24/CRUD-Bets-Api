using BetsApi.Models;
using BetsApi.Repositories;
using Microsoft.EntityFrameworkCore;

namespace BetsApi.Data.Repositories
{
    public class BetRepository : GenericRepository<Bet>, IBetRepository
    {
        public BetRepository(BetsDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Bet>> GetBetsBetweenUsersAsync(long receiverUserId, long senderUserId)
        {
            return await _context.Bets
                .Where(b => b.SenderId == senderUserId && b.ReceiverId == receiverUserId).ToListAsync();
        }

        public async Task<IEnumerable<Bet>> GetPendingBetsAsync()
        {
            return await _context.Bets
                .Where(b => b.BetState == State.Pending).ToListAsync();
        }

        
    }
}
