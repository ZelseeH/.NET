using ChampionsLeagueMaster.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ChampionsLeagueMaster.Controllers
{
    public class SeasonStatsController : Controller
    {
        private readonly ChampionsLeagueMasterContext _context;

        public SeasonStatsController(ChampionsLeagueMasterContext context)
        {
            _context = context;
        }
        public IActionResult Index(string season)
        {
            // Pobierz unikalne, niepuste sezony i posortuj malejąco
            var seasons = _context.SeasonStats
                .Select(s => s.Season)
                .Distinct()
                .Where(s => !string.IsNullOrEmpty(s)) // Usuń puste wartości
                .OrderByDescending(s => s)
                .ToList();

            // Jeśli nie ma sezonów, ustaw domyślnie "2024/2025"
            if (!seasons.Any())
            {
                seasons = new List<string> { "2024/2025" };
            }

            // Ustaw domyślny sezon na najnowszy
            var defaultSeason = seasons.First();
            ViewBag.Seasons = seasons;
            ViewBag.SelectedSeason = season ?? defaultSeason;

            var stats = _context.SeasonStats
                .Include(s => s.Team)
                .Where(s => s.Season == (season ?? defaultSeason));

            // Sortowanie według punktów i różnicy bramek
            stats = stats.OrderByDescending(s => s.Points)
                         .ThenByDescending(s => s.GoalsScored - s.GoalsConceded)
                         .ThenByDescending(s => s.GoalsScored);

            return View(stats.ToList());
        }
    }
}
