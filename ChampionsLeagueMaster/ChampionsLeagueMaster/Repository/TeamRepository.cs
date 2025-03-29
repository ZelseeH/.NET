using ChampionsLeagueMaster.Data;
using ChampionsLeagueMaster.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace ChampionsLeagueMaster.Repository
{
    public class TeamRepository : ITeamRepository
    {
        private readonly ChampionsLeagueMasterContext _context;

        public TeamRepository(ChampionsLeagueMasterContext context)
        {
            _context = context;
        }

        public IQueryable<Team> GetTeamsQueryable()
        {
            return _context.Teams.AsNoTracking().AsQueryable();
        }

        public Task<IQueryable<Team>> GetAllAsync()
        {
            return Task.FromResult(_context.Teams.AsQueryable());
        }

        public async Task<Team?> GetByIdAsync(int id)
        {
            return await _context.Teams.FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task InsertAsync(Team team)
        {
            await _context.Teams.AddAsync(team);
        }

        public Task UpdateAsync(Team team)
        {
            _context.Teams.Update(team);
            return Task.CompletedTask;
        }

        public async Task DeleteAsync(int id)
        {
            var team = await _context.Teams.FindAsync(id);
            if (team != null)
            {
                _context.Teams.Remove(team);
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Teams.AnyAsync(t => t.Id == id);
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }

}
