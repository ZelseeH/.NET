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

        public event Func<string, Task> ResultChanged;

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

        public async Task InsertAsync(Result result)
        {
            await _context.Results.AddAsync(result);
            await SaveAsync();
            // Nie wywoływać tutaj OnResultChanged
        }
        public async Task UpdateAsync(Result result)
        {
            _context.Results.Update(result);
            await SaveAsync();
            await OnResultChanged(result.Season);
        }

        public async Task DeleteAsync(int id)
        {
            var result = await _context.Results.FindAsync(id);
            if (result != null)
            {
                _context.Results.Remove(result);
                await SaveAsync();
                await OnResultChanged(result.Season);
            }
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        private async Task OnResultChanged(string season)
        {
            if (ResultChanged != null)
            {
                await ResultChanged.Invoke(season);
            }
        }
    }
}
