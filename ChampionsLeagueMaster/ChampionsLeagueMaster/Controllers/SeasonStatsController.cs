using ChampionsLeagueMaster.Services;
using ChampionsLeagueMaster.ViewModels.SeasonStats;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace ChampionsLeagueMaster.Controllers
{
    public class SeasonStatsController : Controller
    {
        private readonly ISeasonStatsService _seasonStatsService;

        public SeasonStatsController(ISeasonStatsService seasonStatsService)
        {
            _seasonStatsService = seasonStatsService;
        }

        public async Task<IActionResult> Index(string season)
        {
            var seasons = await _seasonStatsService.GetSeasonsAsync();
            var defaultSeason = await _seasonStatsService.GetDefaultSeasonAsync();
            var selectedSeason = season ?? defaultSeason;

            var statsQuery = await _seasonStatsService.GetSeasonStatsAsync(selectedSeason);
            var stats = await statsQuery.ToListAsync();

            var viewModel = new SeasonStatsListViewModel
            {
                Seasons = seasons,
                SelectedSeason = selectedSeason,
                Stats = stats.Select((s, index) => new SeasonStatsViewModel
                {
                    Position = index + 1,
                    TeamId = s.TeamId ?? 0,
                    TeamName = s.Team?.Name ?? "Nieznany zespół",
                    Played = (s.Wins ?? 0) + (s.Draws ?? 0) + (s.Losses ?? 0),
                    Wins = s.Wins ?? 0,
                    Draws = s.Draws ?? 0,
                    Losses = s.Losses ?? 0,
                    GoalsScored = s.GoalsScored ?? 0,
                    GoalsConceded = s.GoalsConceded ?? 0,
                    Points = s.Points ?? 0
                }).ToList()
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> RegenerateStats(string season)
        {
            await _seasonStatsService.RegenerateSeasonStatsAsync(season);
            TempData["SuccessMessage"] = "Statystyki zostały pomyślnie zaktualizowane.";
            return RedirectToAction(nameof(Index), new { season });
        }
    }
}
