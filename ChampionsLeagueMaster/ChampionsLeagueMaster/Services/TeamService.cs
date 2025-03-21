using ChampionsLeagueMaster.Models;
using ChampionsLeagueMaster.Repository;
using System.Linq;
using System.Threading.Tasks;

namespace ChampionsLeagueMaster.Services
{
    public class TeamService : ITeamService
    {
        private readonly ITeamRepository _teamRepository;

        public TeamService(ITeamRepository teamRepository)
        {
            _teamRepository = teamRepository;
        }

        public async Task<IQueryable<Team>> GetAllTeamsAsync()
        {
            return await _teamRepository.GetAllAsync();
        }

        public async Task<Team?> GetTeamByIdAsync(int id)
        {
            return await _teamRepository.GetByIdAsync(id);
        }

        public async Task CreateTeamAsync(Team team)
        {
            await _teamRepository.InsertAsync(team);
            await _teamRepository.SaveAsync();
        }

        public async Task UpdateTeamAsync(Team team)
        {
            await _teamRepository.UpdateAsync(team);
            await _teamRepository.SaveAsync();
        }

        public async Task DeleteTeamAsync(int id)
        {
            await _teamRepository.DeleteAsync(id);
            await _teamRepository.SaveAsync();
        }

        public async Task<bool> TeamExistsAsync(int id)
        {
            return await _teamRepository.ExistsAsync(id);
        }
    }
}
