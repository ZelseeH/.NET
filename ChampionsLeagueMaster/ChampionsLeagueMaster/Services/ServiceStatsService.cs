using ChampionsLeagueMaster.Models;
using ChampionsLeagueMaster.Repository;
using Microsoft.EntityFrameworkCore;

namespace ChampionsLeagueMaster.Services
{
    public class SeasonStatsService : ISeasonStatsService
    {
        private readonly ISeasonStatsRepository _seasonStatsRepository;

        public SeasonStatsService(ISeasonStatsRepository seasonStatsRepository)
        {
            _seasonStatsRepository = seasonStatsRepository;
        }

        public async Task<IQueryable<SeasonStats>> GetSeasonStatsAsync(string season)
        {
            var stats = await _seasonStatsRepository.GetAllAsync();
            var defaultSeason = await GetDefaultSeasonAsync();

            return stats.Where(s => s.Season == (season ?? defaultSeason))
                        .OrderByDescending(s => s.Points)
                        .ThenByDescending(s => s.GoalsScored - s.GoalsConceded)
                        .ThenByDescending(s => s.GoalsScored);
        }

        public async Task<List<string>> GetSeasonsAsync()
        {
            var stats = await _seasonStatsRepository.GetAllAsync();
            var seasons = await stats
                .Select(s => s.Season)
                .Distinct()
                .Where(s => !string.IsNullOrEmpty(s))
                .OrderByDescending(s => s)
                .ToListAsync();

            return seasons.Any() ? seasons : new List<string> { "2024/2025" };
        }

        public async Task<string> GetDefaultSeasonAsync()
        {
            var seasons = await GetSeasonsAsync();
            return seasons.First();
        }
    }
}
