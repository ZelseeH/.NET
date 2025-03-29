using ChampionsLeagueMaster.Models;
using ChampionsLeagueMaster.Repository;
using ChampionsLeagueMaster.ViewModels.Teams;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChampionsLeagueMaster.Services
{
    public class TeamService : ITeamService
    {
        private readonly ITeamRepository _teamRepository;

        public TeamService(ITeamRepository teamRepository)
        {
            _teamRepository = teamRepository;
        }

        public async Task<PaginatedList<TeamViewModel>> GetPaginatedTeamsAsync(
            string countryFilter,
            string sortOrder,
            int pageIndex,
            int pageSize)
        {
            var query = _teamRepository.GetTeamsQueryable().AsNoTracking();

            if (!string.IsNullOrEmpty(countryFilter))
            {
                query = query.Where(t => t.Country == countryFilter);
            }

            query = sortOrder switch
            {
                "name_desc" => query.OrderByDescending(t => t.Name),
                "country_asc" => query.OrderBy(t => t.Country),
                "country_desc" => query.OrderByDescending(t => t.Country),
                "founded_desc" => query.OrderByDescending(t => t.FoundedAt),
                _ => query.OrderBy(t => t.Name)
            };

            return await PaginatedList<TeamViewModel>.CreateAsync(
                query.Select(t => new TeamViewModel
                {
                    Id = t.Id,
                    Name = t.Name,
                    Country = t.Country,
                    FoundedAt = t.FoundedAt
                }),
                pageIndex,
                pageSize
            );
        }

        public async Task<TeamViewModel?> GetTeamViewModelAsync(int id)
        {
            var team = await _teamRepository.GetByIdAsync(id);
            if (team == null) return null;

            return new TeamViewModel
            {
                Id = team.Id,
                Name = team.Name,
                Country = team.Country,
                FoundedAt = team.FoundedAt
            };
        }

        public async Task CreateTeamAsync(TeamCreateEditViewModel teamViewModel)
        {
            var team = new Team
            {
                Name = teamViewModel.Name,
                Country = teamViewModel.Country,
                FoundedAt = teamViewModel.FoundedAt
            };

            await _teamRepository.InsertAsync(team);
            await _teamRepository.SaveAsync();
        }

        public async Task UpdateTeamAsync(TeamCreateEditViewModel teamViewModel)
        {
            var team = await _teamRepository.GetByIdAsync(teamViewModel.Id);
            if (team == null) throw new KeyNotFoundException("Team not found");

            team.Name = teamViewModel.Name;
            team.Country = teamViewModel.Country;
            team.FoundedAt = teamViewModel.FoundedAt;

            await _teamRepository.UpdateAsync(team);
            await _teamRepository.SaveAsync();
        }

        public async Task DeleteTeamAsync(int id)
        {
            await _teamRepository.DeleteAsync(id);
            await _teamRepository.SaveAsync();
        }

        public async Task<bool> TeamExistsAsync(int id)
        {
            return await _teamRepository.ExistsAsync(id);
        }

        public async Task<List<string>> GetAvailableCountriesAsync()
        {
            return await _teamRepository.GetTeamsQueryable()
                .Select(t => t.Country)
                .Distinct()
                .OrderBy(c => c)
                .ToListAsync();
        }
    }
}