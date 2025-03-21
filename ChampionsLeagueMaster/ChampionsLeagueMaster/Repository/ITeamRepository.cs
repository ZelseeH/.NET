using ChampionsLeagueMaster.Models;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace ChampionsLeagueMaster.Repository
{
    public interface ITeamRepository
    {
        Task<IQueryable<Team>> GetAllAsync();
        Task<Team?> GetByIdAsync(int id);
        Task InsertAsync(Team team);
        Task UpdateAsync(Team team);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task SaveAsync();
    }
}
