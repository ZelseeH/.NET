using ChampionsLeagueMaster.Models;
using ChampionsLeagueMaster.ViewModels.Players;
using Microsoft.AspNetCore.Mvc.Rendering;

public interface IPlayerService
{
    Task<IQueryable<Player>> GetAllPlayersAsync(string teamName, string position, string sortOrder);
    Task<Player?> GetPlayerByIdAsync(int playerId);
    Task<PlayerViewModel> GetPlayerViewModelByIdAsync(int playerId);
    Task<SelectList> GetTeamsSelectListAsync(int? selectedTeamId = null);
    Task<List<string>> GetAvailablePositionsAsync();
    Task CreatePlayerAsync(PlayerCreateEditViewModel playerViewModel);
    Task UpdatePlayerAsync(PlayerCreateEditViewModel playerViewModel);
    Task DeletePlayerAsync(int playerId);
    Task<PaginatedList<PlayerViewModel>> GetPaginatedPlayerViewModelsAsync(string teamName,
       string position, string sortOrder, int pageIndex, int pageSize);
    Task<PlayerCreateEditViewModel> GetPlayerForEditAsync(int playerId);
}
