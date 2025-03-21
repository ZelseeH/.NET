using ChampionsLeagueMaster.Models;
using System.Linq;
using System.Threading.Tasks;

namespace ChampionsLeagueMaster.Services
{
    public interface ITeamService
    {
        Task<IQueryable<Team>> GetAllTeamsAsync();
        Task<Team?> GetTeamByIdAsync(int id);
        Task CreateTeamAsync(Team team);
        Task UpdateTeamAsync(Team team);
        Task DeleteTeamAsync(int id);
        Task<bool> TeamExistsAsync(int id);
    }
}
