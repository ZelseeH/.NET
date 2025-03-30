using ChampionsLeagueMaster.Models;
using ChampionsLeagueMaster.ViewModels.Results;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ChampionsLeagueMaster.Services
{
    public interface IResultService
    {
        Task<List<ResultViewModel>> GetFilteredResultsAsync(string season, string round);
        Task<Result> GetResultByIdAsync(int id);
        Task<ResultViewModel> GetResultViewModelByIdAsync(int id);
        Task<ResultCreateEditViewModel> GetResultForEditAsync(int id);
        Task<List<string>> GetSeasonsAsync();
        Task<List<string>> GetRoundsAsync();
        Task<SelectList> GetTeamsSelectListAsync();
        Task CreateResultAsync(ResultCreateEditViewModel resultViewModel);
        Task UpdateResultAsync(ResultCreateEditViewModel resultViewModel);
        Task DeleteResultAsync(int id);
        Task<List<string>> GetRoundsBySeasonAsync(string season);

    }
}
