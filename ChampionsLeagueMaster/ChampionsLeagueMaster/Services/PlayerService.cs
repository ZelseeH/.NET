using ChampionsLeagueMaster.Models;
using ChampionsLeagueMaster.Repository;
using ChampionsLeagueMaster.ViewModels.Players;
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

        public async Task<PlayerViewModel> GetPlayerViewModelByIdAsync(int playerId)
        {
            var player = await _playerRepository.GetByIdAsync(playerId);
            if (player == null)
                return null;

            return new PlayerViewModel
            {
                Id = player.Id,
                FirstName = player.FirstName,
                LastName = player.LastName,
                TeamId = player.TeamId,
                TeamName = player.Team?.Name,
                Position = player.Position,
                JerseyNumber = player.JerseyNumber,
                BirthDate = player.BirthDate,
                Country = player.Country
            };
        }

        public async Task<PlayerCreateEditViewModel> GetPlayerForEditAsync(int playerId)
        {
            var player = await _playerRepository.GetByIdAsync(playerId);
            if (player == null)
                return null;

            return new PlayerCreateEditViewModel
            {
                Id = player.Id,
                FirstName = player.FirstName,
                LastName = player.LastName,
                TeamId = player.TeamId,
                Position = player.Position,
                JerseyNumber = player.JerseyNumber,
                BirthDate = player.BirthDate,
                Country = player.Country,
                Teams = await GetTeamsSelectListAsync(player.TeamId)
            };
        }

        public async Task<SelectList> GetTeamsSelectListAsync(int? selectedTeamId = null)
        {
            var teams = await _teamRepository.GetAllAsync();
            return new SelectList(await teams.ToListAsync(), "Id", "Name", selectedTeamId);
        }

        public async Task CreatePlayerAsync(PlayerCreateEditViewModel playerViewModel)
        {
            var player = new Player
            {
                FirstName = playerViewModel.FirstName,
                LastName = playerViewModel.LastName,
                TeamId = playerViewModel.TeamId,
                Position = playerViewModel.Position,
                JerseyNumber = playerViewModel.JerseyNumber,
                BirthDate = playerViewModel.BirthDate,
                Country = playerViewModel.Country
            };

            await _playerRepository.InsertAsync(player);
            await _playerRepository.SaveAsync();
        }

        public async Task UpdatePlayerAsync(PlayerCreateEditViewModel playerViewModel)
        {
            var player = await _playerRepository.GetByIdAsync(playerViewModel.Id);
            if (player == null)
                throw new KeyNotFoundException($"Player with ID {playerViewModel.Id} not found");

            player.FirstName = playerViewModel.FirstName;
            player.LastName = playerViewModel.LastName;
            player.TeamId = playerViewModel.TeamId;
            player.Position = playerViewModel.Position;
            player.JerseyNumber = playerViewModel.JerseyNumber;
            player.BirthDate = playerViewModel.BirthDate;
            player.Country = playerViewModel.Country;

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

        public async Task<PaginatedList<PlayerViewModel>> GetPaginatedPlayerViewModelsAsync(string teamName, string position, string sortOrder, int pageIndex, int pageSize)
        {
            var players = await GetAllPlayersAsync(teamName, position, sortOrder);

        
            var playerViewModels = players.Select(p => new PlayerViewModel
            {
                Id = p.Id,
                FirstName = p.FirstName,
                LastName = p.LastName,
                TeamId = p.TeamId,
                TeamName = p.Team.Name,
                Position = p.Position,
                JerseyNumber = p.JerseyNumber,
                BirthDate = p.BirthDate,
                Country = p.Country
            });

            
            return await PaginatedList<PlayerViewModel>.CreateAsync(playerViewModels, pageIndex, pageSize);
        }
    }
}
