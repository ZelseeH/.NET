using ChampionsLeagueMaster.Models;
using ChampionsLeagueMaster.Repository;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ChampionsLeagueMaster.Services
{
    public class PlayerService : IPlayerService
    {
        private readonly IPlayerRepository _playerRepository;
        private readonly ITeamRepository _teamRepository;

        public PlayerService(IPlayerRepository playerRepository, ITeamRepository teamRepository)
        {
            _playerRepository = playerRepository;
            _teamRepository = teamRepository;
        }

        public async Task<IQueryable<Player>> GetAllPlayersAsync(string teamName, string position, string sortOrder)
        {
            var players = await _playerRepository.GetAllAsync();

            if (!string.IsNullOrEmpty(teamName))
            {
                players = players.Where(p => p.Team.Name.Contains(teamName));
            }

            if (!string.IsNullOrEmpty(position))
            {
                players = players.Where(p => p.Position == position);
            }

            players = sortOrder switch
            {
                "firstname_desc" => players.OrderByDescending(p => p.FirstName),
                "lastname_asc" => players.OrderBy(p => p.LastName),
                "lastname_desc" => players.OrderByDescending(p => p.LastName),
                _ => players.OrderBy(p => p.FirstName),
            };

            return players;
        }

        public async Task<Player?> GetPlayerByIdAsync(int playerId)
        {
            return await _playerRepository.GetByIdAsync(playerId);
        }

        public async Task<SelectList> GetTeamsSelectListAsync(int? selectedTeamId = null)
        {
            var teams = await _teamRepository.GetAllAsync();
            return new SelectList(await teams.ToListAsync(), "Id", "Name", selectedTeamId);
        }

        public async Task CreatePlayerAsync(Player player)
        {
            await _playerRepository.InsertAsync(player);
            await _playerRepository.SaveAsync();
        }

        public async Task UpdatePlayerAsync(Player player)
        {
            await _playerRepository.UpdateAsync(player);
            await _playerRepository.SaveAsync();
        }

        public async Task DeletePlayerAsync(int playerId)
        {
            await _playerRepository.DeleteAsync(playerId);
            await _playerRepository.SaveAsync();
        }

        public async Task<List<string>> GetAvailablePositionsAsync()
        {
            var players = await _playerRepository.GetAllAsync();
            return await players
                .Select(p => p.Position)
                .Where(p => !string.IsNullOrEmpty(p))
                .Distinct()
                .OrderBy(p => p)
                .ToListAsync();
        }

    }
}
