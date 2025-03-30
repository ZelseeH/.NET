using ChampionsLeagueMaster.Models;

namespace ChampionsLeagueMaster.Repository
{
    public interface ISeasonStatsRepository
    {
        Task<IQueryable<SeasonStats>> GetAllAsync();
        Task<SeasonStats?> GetByIdAsync(int id);
        Task InsertAsync(SeasonStats seasonStats);
        Task UpdateAsync(SeasonStats seasonStats);
        Task DeleteAsync(int id);
        Task DeleteBySeasonAsync(string season);
        Task SaveAsync();
    }
}
