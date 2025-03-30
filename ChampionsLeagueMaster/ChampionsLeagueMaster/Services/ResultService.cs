using ChampionsLeagueMaster.Models;
using ChampionsLeagueMaster.Repository;
using ChampionsLeagueMaster.ViewModels.Results;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ChampionsLeagueMaster.Services
{
    public class ResultService : IResultService
    {
        private readonly IResultRepository _resultRepository;
        private readonly ITeamRepository _teamRepository;
        private readonly ISeasonStatsService _seasonStatsService;

        public ResultService(
        IResultRepository resultRepository,
        ITeamRepository teamRepository,
        ISeasonStatsService seasonStatsService)
        {
            _resultRepository = resultRepository;
            _teamRepository = teamRepository;
            _seasonStatsService = seasonStatsService;

           
        }

        public async Task<IQueryable<Result>> GetAllResultsAsync(string season, string round)
        {
            var results = await _resultRepository.GetAllAsync();

            if (!string.IsNullOrEmpty(season))
                results = results.Where(r => r.Season == season);

            if (!string.IsNullOrEmpty(round))
                results = results.Where(r => r.Round == round);

            return results;
        }

        public async Task<Result?> GetResultByIdAsync(int id)
        {
            return await _resultRepository.GetByIdAsync(id);
        }

        public async Task<List<string>> GetSeasonsAsync()
        {
            var results = await _resultRepository.GetAllAsync();
            var seasons = await results
                .Select(r => r.Season)
                .Distinct()
                .OrderByDescending(s => s)
                .ToListAsync();

            return seasons.Any() ? seasons : new List<string> { "2024/2025" };
        }

        public async Task<List<string>> GetRoundsAsync()
        {
            var results = await _resultRepository.GetAllAsync();
            var rounds = await results
                .Select(r => r.Round)
                .Distinct()
                .OrderByDescending(r => r)
                .ToListAsync();

            return rounds.Any() ? rounds : new List<string> { "Grupa" };
        }

        public async Task<SelectList> GetTeamsSelectListAsync()
        {
            var teams = await _teamRepository.GetAllAsync();
            return new SelectList(await teams.ToListAsync(), "Id", "Name");
        }

        public async Task CreateResultAsync(ResultCreateEditViewModel resultViewModel)
        {
            var result = new Result
            {
                HomeTeamId = resultViewModel.HomeTeamId,
                AwayTeamId = resultViewModel.AwayTeamId,
                Season = resultViewModel.Season,
                HomeTeamGoals = resultViewModel.HomeTeamGoals,
                AwayTeamGoals = resultViewModel.AwayTeamGoals,
                MatchDay = resultViewModel.MatchDay,
                MatchTime = resultViewModel.MatchTime,
                Status = resultViewModel.Status,
                Round = resultViewModel.Round
            };

            await _resultRepository.InsertAsync(result);
            await _resultRepository.SaveAsync();

            await _seasonStatsService.RegenerateSeasonStatsAsync(result.Season);
        }

        public async Task UpdateResultAsync(ResultCreateEditViewModel resultViewModel)
        {
            var result = await _resultRepository.GetByIdAsync(resultViewModel.Id);
            if (result == null) throw new KeyNotFoundException($"Result with ID {resultViewModel.Id} not found");

            string originalSeason = result.Season;

            result.HomeTeamId = resultViewModel.HomeTeamId;
            result.AwayTeamId = resultViewModel.AwayTeamId;
            result.Season = resultViewModel.Season;
            result.HomeTeamGoals = resultViewModel.HomeTeamGoals;
            result.AwayTeamGoals = resultViewModel.AwayTeamGoals;
            result.MatchDay = resultViewModel.MatchDay;
            result.MatchTime = resultViewModel.MatchTime;
            result.Status = resultViewModel.Status;
            result.Round = resultViewModel.Round;

            await _resultRepository.UpdateAsync(result);
            await _resultRepository.SaveAsync();

            await _seasonStatsService.RegenerateSeasonStatsAsync(result.Season);

            if (originalSeason != result.Season)
            {
                await _seasonStatsService.RegenerateSeasonStatsAsync(originalSeason);
            }
        }

        public async Task DeleteResultAsync(int id)
        {
            var result = await _resultRepository.GetByIdAsync(id);
            if (result == null) return;

            string season = result.Season;

            await _resultRepository.DeleteAsync(id);
            await _resultRepository.SaveAsync();

            await _seasonStatsService.RegenerateSeasonStatsAsync(season);
        }

        public async Task<List<ResultViewModel>> GetFilteredResultsAsync(string season, string round)
        {
            var results = await GetAllResultsAsync(season, round);

            return await results.Select(r => new ResultViewModel
            {
                Id = r.Id,
                HomeTeamName = r.HomeTeam.Name ?? "Nieznany zespół",
                AwayTeamName = r.AwayTeam.Name ?? "Nieznany zespół",
                Season = r.Season,
                HomeTeamGoals = r.HomeTeamGoals,
                AwayTeamGoals = r.AwayTeamGoals,
                MatchDay = r.MatchDay,
                MatchTime = r.MatchTime,
                Status = r.Status,
                Round = r.Round
            }).ToListAsync();
        }

        public async Task<ResultViewModel> GetResultViewModelByIdAsync(int id)
        {
            var result = await GetResultByIdAsync(id);
            if (result == null) return null;

            return new ResultViewModel
            {
                Id = result.Id,
                HomeTeamName = result.HomeTeam?.Name ?? "Nieznany zespół",
                AwayTeamName = result.AwayTeam?.Name ?? "Nieznany zespół",
                Season = result.Season,
                HomeTeamGoals = result.HomeTeamGoals,
                AwayTeamGoals = result.AwayTeamGoals,
                MatchDay = result.MatchDay,
                MatchTime = result.MatchTime,
                Status = result.Status,
                Round = result.Round
            };
        }

        public async Task<ResultCreateEditViewModel> GetResultForEditAsync(int id)
        {
            var result = await GetResultByIdAsync(id);
            if (result == null) return null;

            return new ResultCreateEditViewModel
            {
                Id = result.Id,
                HomeTeamId = result.HomeTeamId,
                AwayTeamId = result.AwayTeamId,
                Season = result.Season,
                HomeTeamGoals = result.HomeTeamGoals,
                AwayTeamGoals = result.AwayTeamGoals,
                MatchDay = result.MatchDay,
                MatchTime = result.MatchTime,
                Status = result.Status,
                Round = result.Round,
                Teams = await GetTeamsSelectListAsync()
            };
        }
        public async Task<List<string>> GetRoundsBySeasonAsync(string season)
        {
            if (string.IsNullOrEmpty(season))
                return await GetRoundsAsync();

            var results = await _resultRepository.GetAllAsync();
            var rounds = await results
                .Where(r => r.Season == season)
                .Select(r => r.Round)
                .Distinct()
                .OrderBy(r => r)
                .ToListAsync();

            return rounds.Any() ? rounds : new List<string> { "Grupa" };
        }

    }
}
