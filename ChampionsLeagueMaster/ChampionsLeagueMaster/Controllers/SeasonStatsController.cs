using ChampionsLeagueMaster.Models;
using ChampionsLeagueMaster.Services;
using Microsoft.AspNetCore.Mvc;
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

            ViewBag.Seasons = seasons;
            ViewBag.SelectedSeason = season ?? defaultSeason;

            var stats = await _seasonStatsService.GetSeasonStatsAsync(season);

            return View(stats);
        }
    }
}
