using ChampionsLeagueMaster.Data;
using ChampionsLeagueMaster.Models;
using Microsoft.EntityFrameworkCore;

namespace ChampionsLeagueMaster.Repository
{
    public class SeasonStatsRepository : ISeasonStatsRepository
    {
        private readonly ChampionsLeagueMasterContext _context;

        public SeasonStatsRepository(ChampionsLeagueMasterContext context)
        {
            _context = context;
        }

        public Task<IQueryable<SeasonStats>> GetAllAsync()
        {
            return Task.FromResult(_context.SeasonStats.Include(s => s.Team).AsQueryable());
        }

        public async Task<SeasonStats?> GetByIdAsync(int id)
        {
            return await _context.SeasonStats
                .Include(s => s.Team)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task InsertAsync(SeasonStats seasonStats)
        {
            await _context.SeasonStats.AddAsync(seasonStats);
        }

        public Task UpdateAsync(SeasonStats seasonStats)
        {
            _context.SeasonStats.Update(seasonStats);
            return Task.CompletedTask;
        }

        public async Task DeleteAsync(int id)
        {
            var seasonStats = await _context.SeasonStats.FindAsync(id);
            if (seasonStats != null)
            {
                _context.SeasonStats.Remove(seasonStats);
            }
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
