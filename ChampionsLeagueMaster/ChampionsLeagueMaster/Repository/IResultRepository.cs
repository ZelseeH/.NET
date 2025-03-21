using ChampionsLeagueMaster.Models;

namespace ChampionsLeagueMaster.Repository
{
    public interface IResultRepository
    {
        Task<IQueryable<Result>> GetAllAsync();
        Task<Result?> GetByIdAsync(int id);
        Task<List<string>> GetSeasonsAsync();
        Task<List<string>> GetRoundsAsync();
        Task InsertAsync(Result result);
        Task UpdateAsync(Result result);
        Task DeleteAsync(int id);
        Task SaveAsync();
    }
}
