using ChampionsLeagueMaster.Models;
using ChampionsLeagueMaster.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ChampionsLeagueMaster.Controllers
{
    public class SeasonStatsController : Controller
    {
        private readonly ISeasonStatsRepository _seasonStatsRepository;

        public SeasonStatsController(ISeasonStatsRepository seasonStatsRepository)
        {
            _seasonStatsRepository = seasonStatsRepository;
        }

        public async Task<IActionResult> Index(string season)
        {
            var seasons = await _seasonStatsRepository.GetSeasonsAsync();

            if (!seasons.Any())
            {
                seasons = new List<string> { "2024/2025" };
            }

            var defaultSeason = seasons.First();
            ViewBag.Seasons = seasons;
            ViewBag.SelectedSeason = season ?? defaultSeason;

            var stats = await _seasonStatsRepository.GetAllAsync();

            stats = stats.Where(s => s.Season == (season ?? defaultSeason))
                         .OrderByDescending(s => s.Points)
                         .ThenByDescending(s => s.GoalsScored - s.GoalsConceded)
                         .ThenByDescending(s => s.GoalsScored);

            return View(await stats.ToListAsync());
        }
    }
}
