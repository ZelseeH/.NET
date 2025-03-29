using ChampionsLeagueMaster.Models;
using ChampionsLeagueMaster.ViewModels.Teams;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ChampionsLeagueMaster.Services
{
    public interface ITeamService
    {
        Task<PaginatedList<TeamViewModel>> GetPaginatedTeamsAsync(string countryFilter, string sortOrder, int pageIndex, int pageSize);
        Task<TeamViewModel?> GetTeamViewModelAsync(int id);
        Task CreateTeamAsync(TeamCreateEditViewModel teamViewModel);
        Task UpdateTeamAsync(TeamCreateEditViewModel teamViewModel);
        Task DeleteTeamAsync(int id);
        Task<bool> TeamExistsAsync(int id);
        Task<List<string>> GetAvailableCountriesAsync();
    }
}