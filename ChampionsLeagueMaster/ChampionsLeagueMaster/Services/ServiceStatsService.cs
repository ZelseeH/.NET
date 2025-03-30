using ChampionsLeagueMaster.Models;
using ChampionsLeagueMaster.Repository;
using ChampionsLeagueMaster.ViewModels.SeasonStats;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChampionsLeagueMaster.Services
{
    public class SeasonStatsService : ISeasonStatsService
    {
        private readonly ISeasonStatsRepository _seasonStatsRepository;
        private readonly IResultRepository _resultsRepository;

        public SeasonStatsService(ISeasonStatsRepository seasonStatsRepository, IResultRepository resultsRepository)
        {
            _seasonStatsRepository = seasonStatsRepository;
            _resultsRepository = resultsRepository;

            _resultsRepository.ResultChanged += RegenerateSeasonStatsAsync;
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

            if (!seasons.Any())
            {
                var results = await _resultsRepository.GetAllAsync();
                seasons = await results
                    .Select(r => r.Season)
                    .Distinct()
                    .Where(s => !string.IsNullOrEmpty(s))
                    .OrderByDescending(s => s)
                    .ToListAsync();
            }

            return seasons.Any() ? seasons : new List<string> { "2024/2025" };
        }

        public async Task<string> GetDefaultSeasonAsync()
        {
            var seasons = await GetSeasonsAsync();
            return seasons.First();
        }

        public async Task RegenerateSeasonStatsAsync(string season)
        {
            if (string.IsNullOrEmpty(season))
                return;

            await _seasonStatsRepository.DeleteBySeasonAsync(season);
            await _seasonStatsRepository.SaveAsync();

            var results = await _resultsRepository.GetAllAsync();
            var seasonResults = results.Where(r => r.Season == season).ToList();

            var newStats = new Dictionary<int, SeasonStats>();

            foreach (var result in seasonResults)
            {
                if (result.HomeTeamId.HasValue && result.AwayTeamId.HasValue &&
                    result.HomeTeamGoals.HasValue && result.AwayTeamGoals.HasValue)
                {
                    UpdateTeamStats(newStats, result.HomeTeamId.Value, result.Season,
                                   result.HomeTeamGoals.Value, result.AwayTeamGoals.Value);
                    UpdateTeamStats(newStats, result.AwayTeamId.Value, result.Season,
                                   result.AwayTeamGoals.Value, result.HomeTeamGoals.Value);
                }
            }

            foreach (var stat in newStats.Values)
            {
                await _seasonStatsRepository.InsertAsync(stat);
            }

            await _seasonStatsRepository.SaveAsync();
        }

        private void UpdateTeamStats(Dictionary<int, SeasonStats> stats, int teamId, string season, int goalsFor, int goalsAgainst)
        {
            if (!stats.TryGetValue(teamId, out var teamStats))
            {
                teamStats = new SeasonStats
                {
                    TeamId = teamId,
                    Season = season,
                    Wins = 0,
                    Draws = 0,
                    Losses = 0,
                    Points = 0,
                    GoalsScored = 0,
                    GoalsConceded = 0
                };
                stats[teamId] = teamStats;
            }

            teamStats.GoalsScored += goalsFor;
            teamStats.GoalsConceded += goalsAgainst;

            if (goalsFor > goalsAgainst)
            {
                teamStats.Wins++;
                teamStats.Points += 3;
            }
            else if (goalsFor == goalsAgainst)
            {
                teamStats.Draws++;
                teamStats.Points += 1;
            }
            else
            {
                teamStats.Losses++;
            }
        }
    }
}
