using ChampionsLeagueMaster.Data;
using ChampionsLeagueMaster.Models;
using Microsoft.EntityFrameworkCore;

namespace ChampionsLeagueMaster.Repository
{
    public class ResultRepository : IResultRepository
    {
        private readonly ChampionsLeagueMasterContext _context;

        public ResultRepository(ChampionsLeagueMasterContext context)
        {
            _context = context;
        }

        public Task<IQueryable<Result>> GetAllAsync()
        {
            return Task.FromResult(_context.Results
                .Include(r => r.HomeTeam)
                .Include(r => r.AwayTeam)
                .AsQueryable());
        }

        public async Task<Result?> GetByIdAsync(int id)
        {
            return await _context.Results
                .Include(r => r.HomeTeam)
                .Include(r => r.AwayTeam)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<List<string>> GetSeasonsAsync()
        {
            return await _context.Results
                .Select(r => r.Season)
                .Distinct()
                .OrderByDescending(s => s)
                .ToListAsync();
        }

        public async Task<List<string>> GetRoundsAsync()
        {
            return await _context.Results
                .Select(r => r.Round)
                .Distinct()
                .OrderByDescending(r => r)
                .ToListAsync();
        }

        public async Task InsertAsync(Result result)
        {
            await _context.Results.AddAsync(result);
        }

        public Task UpdateAsync(Result result)
        {
            _context.Results.Update(result);
            return Task.CompletedTask;
        }

        public async Task DeleteAsync(int id)
        {
            var result = await _context.Results.FindAsync(id);
            if (result != null)
            {
                _context.Results.Remove(result);
            }
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
