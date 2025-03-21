using ChampionsLeagueMaster.Data;
using ChampionsLeagueMaster.Models;
using Microsoft.EntityFrameworkCore;

namespace ChampionsLeagueMaster.Repository
{
    public class PlayerRepository : IPlayerRepository
    {
        private readonly ChampionsLeagueMasterContext _context;

        public PlayerRepository(ChampionsLeagueMasterContext context)
        {
            _context = context;
        }

        public Task<IQueryable<Player>> GetAllAsync()
        {
            return Task.FromResult(_context.Players.Include(p => p.Team).AsQueryable());
        }

        public async Task<Player?> GetByIdAsync(int playerId)
        {
            return await _context.Players
                .Include(p => p.Team)
                .FirstOrDefaultAsync(p => p.Id == playerId);
        }

        public async Task InsertAsync(Player player)
        {
            await _context.Players.AddAsync(player);
        }

        public Task UpdateAsync(Player player)
        {
            _context.Players.Update(player);
            return Task.CompletedTask;
        }

        public async Task DeleteAsync(int playerId)
        {
            var player = await _context.Players.FindAsync(playerId);
            if (player != null)
            {
                _context.Players.Remove(player);
            }
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
