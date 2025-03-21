using ChampionsLeagueMaster.Models;
using ChampionsLeagueMaster.Repository;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ChampionsLeagueMaster.Services
{
    public class ResultService : IResultService
    {
        private readonly IResultRepository _resultRepository;
        private readonly ITeamRepository _teamRepository;

        public ResultService(IResultRepository resultRepository, ITeamRepository teamRepository)
        {
            _resultRepository = resultRepository;
            _teamRepository = teamRepository;
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
            return await results
                .Select(r => r.Season)
                .Distinct()
                .OrderByDescending(s => s)
                .ToListAsync();
        }

        public async Task<List<string>> GetRoundsAsync()
        {
            var results = await _resultRepository.GetAllAsync();
            return await results
                .Select(r => r.Round)
                .Distinct()
                .OrderByDescending(r => r)
                .ToListAsync();
        }

        public async Task<List<SelectListItem>> GetTeamsSelectListAsync()
        {
            var teams = await _teamRepository.GetAllAsync();
            if (teams == null || !teams.Any())
            {
                return new List<SelectListItem>();
            }

            return await teams.Select(t => new SelectListItem
            {
                Value = t.Id.ToString(),
                Text = t.Name ?? "Unknown Team"
            }).ToListAsync();
        }


        public async Task CreateResultAsync(Result result)
        {
            await _resultRepository.InsertAsync(result);
            await _resultRepository.SaveAsync();
        }

        public async Task UpdateResultAsync(Result result)
        {
            await _resultRepository.UpdateAsync(result);
            await _resultRepository.SaveAsync();
        }

        public async Task DeleteResultAsync(int id)
        {
            await _resultRepository.DeleteAsync(id);
            await _resultRepository.SaveAsync();
        }
    }
}
