using ChampionsLeagueMaster.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

public interface IPlayerService
{
    Task<IQueryable<Player>> GetAllPlayersAsync(string teamName, string position, string sortOrder);
    Task<Player?> GetPlayerByIdAsync(int playerId);
    Task<SelectList> GetTeamsSelectListAsync(int? selectedTeamId = null);
    Task<List<string>> GetAvailablePositionsAsync();
    Task CreatePlayerAsync(Player player);
    Task UpdatePlayerAsync(Player player);
    Task DeletePlayerAsync(int playerId);
}
