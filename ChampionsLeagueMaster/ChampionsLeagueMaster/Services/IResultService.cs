using ChampionsLeagueMaster.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ChampionsLeagueMaster.Services
{
    public interface IResultService
    {
        Task<IQueryable<Result>> GetAllResultsAsync(string season, string round);
        Task<Result?> GetResultByIdAsync(int id);
        Task<List<string>> GetSeasonsAsync();
        Task<List<string>> GetRoundsAsync();
        Task<List<SelectListItem>> GetTeamsSelectListAsync();
        Task CreateResultAsync(Result result);
        Task UpdateResultAsync(Result result);
        Task DeleteResultAsync(int id);
    }
}
