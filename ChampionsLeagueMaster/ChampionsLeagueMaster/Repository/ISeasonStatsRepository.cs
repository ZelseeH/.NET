using ChampionsLeagueMaster.Models;
using System.Linq.Expressions;

namespace ChampionsLeagueMaster.Repository
{
    public interface ISeasonStatsRepository
    {
        Task<IQueryable<SeasonStats>> GetAllAsync();
        Task<SeasonStats?> GetByIdAsync(int id);
        Task<List<string>> GetSeasonsAsync();
        Task InsertAsync(SeasonStats seasonStats);
        Task UpdateAsync(SeasonStats seasonStats);
        Task DeleteAsync(int id);
        Task SaveAsync();
    }
}
