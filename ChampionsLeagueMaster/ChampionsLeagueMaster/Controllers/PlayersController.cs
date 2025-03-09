using ChampionsLeagueMaster.Data;
using ChampionsLeagueMaster.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace ChampionsLeagueMaster.Controllers
{
    public class PlayersController : Controller
    {
        private readonly ChampionsLeagueMasterContext _context;

        public PlayersController(ChampionsLeagueMasterContext context)
        {
            _context = context;
        }
        private bool PlayerExists(int id)
        {
            return _context.Players.Any(e => e.Id == id);
        }
        public async Task<IActionResult> Index(string teamName, string position, string sortOrder)
        {
            var players = _context.Players.Include(p => p.Team).AsQueryable();
                 
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

                       var availablePositions = await _context.Players
                .Select(p => p.Position)
                .Distinct()
                .ToListAsync();

            ViewData["AvailablePositions"] = availablePositions;
            ViewData["CurrentTeamFilter"] = teamName;
            ViewData["CurrentPositionFilter"] = position;
            ViewData["CurrentSortOrder"] = sortOrder;

            return View(await players.ToListAsync());
        }


        public async Task<IActionResult> Create()
        {
            var teams = await _context.Teams.ToListAsync();
            ViewData["Teams"] = teams.Any()
                ? new SelectList(teams, "Id", "Name")
                : new SelectList(new List<Team>(), "Id", "Name");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Player player)
        {
                 Console.WriteLine($"DEBUG: Player TeamId = {player.TeamId}");

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Entry(player).State = EntityState.Added;
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException ex)
                {
                    ModelState.AddModelError("", $"Błąd zapisu do bazy danych: {ex.InnerException?.Message ?? ex.Message}");
                }
            }
            var teamExists = await _context.Teams.AnyAsync(t => t.Id == player.TeamId);
            Console.WriteLine($"DEBUG: Team exists? {teamExists}");
            var teams = await _context.Teams.ToListAsync();
            ViewData["Teams"] = new SelectList(teams, "Id", "Name");

            return View(player);
        }
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var player = await _context.Players
                .Include(p => p.Team) 
                .FirstOrDefaultAsync(p => p.Id == id);

            if (player == null)
            {
                return NotFound();
            }

            var teams = await _context.Teams.ToListAsync();
            ViewData["Teams"] = teams.Any()
                ? new SelectList(teams, "Id", "Name", player.TeamId)
                : new SelectList(new List<Team>(), "Id", "Name");

            return View(player);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Player player)
        {
            if (id != player.Id)
            {  return NotFound(); }

            if (ModelState.IsValid)
            {
              
                    var existingPlayer = await _context.Players.FindAsync(id);
                    if (existingPlayer == null)
                    {
                        return NotFound();
                    }
                    existingPlayer.FirstName = player.FirstName;
                    existingPlayer.LastName = player.LastName;
                    existingPlayer.TeamId = player.TeamId; 
                    existingPlayer.Position = player.Position;
                    existingPlayer.JerseyNumber = player.JerseyNumber;
                    existingPlayer.BirthDate = player.BirthDate;

                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
            }

            var teams = await _context.Teams.ToListAsync();
            ViewData["Teams"] = new SelectList(teams, "Id", "Name", player.TeamId);
            return View(player);
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var player = await _context.Players
                .Include(p => p.Team) 
                .FirstOrDefaultAsync(p => p.Id == id);

            if (player == null)
            {
                return NotFound();
            }

            return View(player);
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var player = await _context.Players
                .Include(p => p.Team) 
                .FirstOrDefaultAsync(p => p.Id == id);

            if (player == null)
            {
                return NotFound();
            }

            return View(player);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var player = await _context.Players.FindAsync(id);
            if (player == null)
            {  return NotFound(); }

           
                _context.Players.Remove(player);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            
        }
    }
}
