using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ChampionsLeagueMaster.Data;
using ChampionsLeagueMaster.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ChampionsLeagueMaster.Controllers
{
    public class ResultsController : Controller
    {
        private readonly ChampionsLeagueMasterContext _context;

        public ResultsController(ChampionsLeagueMasterContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string season, string round)
        {
            var seasons = await _context.Results
                .Select(r => r.Season)
                .Distinct()
                .OrderByDescending(s => s)
                .ToListAsync();

            var rounds = await _context.Results
                .Select(r => r.Round)
                .Distinct()
                .OrderByDescending(r => r)
                .ToListAsync();

            ViewBag.Seasons = seasons;
            ViewBag.Rounds = rounds;
            ViewBag.SelectedSeason = season ?? seasons.FirstOrDefault();
            ViewBag.SelectedRound = round ?? rounds.FirstOrDefault();

            var results = _context.Results
                .Include(r => r.HomeTeam)
                .Include(r => r.AwayTeam)
                .AsQueryable();

            if (!string.IsNullOrEmpty(season))
            {
                results = results.Where(r => r.Season == season);
            }
            else
            {
                results = results.Where(r => r.Season == seasons.FirstOrDefault());
            }

            if (!string.IsNullOrEmpty(round))
            {
                results = results.Where(r => r.Round == round);
            }
            else
            {
                results = results.Where(r => r.Round == rounds.FirstOrDefault());
            }

            return View(await results.ToListAsync());
        }

        public IActionResult Create()
        {
            ViewBag.Teams = _context.Teams.ToList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("HomeTeamId,AwayTeamId,Season,HomeTeamGoals,AwayTeamGoals,MatchDay,MatchTime,Status,Round")] Result result)
        {
            if (ModelState.IsValid)
            {
                _context.Add(result);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Teams = _context.Teams.ToList();
            return View(result);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var result = await _context.Results.FindAsync(id);
            if (result == null) return NotFound();

            ViewBag.Teams = _context.Teams
                .Select(t => new SelectListItem
                {
                    Value = t.Id.ToString(),
                    Text = t.Name
                })
                .ToList();
            return View(result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,HomeTeamId,AwayTeamId,Season,HomeTeamGoals,AwayTeamGoals,MatchDay,MatchTime,Status,Round")] Result result)
        {
            if (id != result.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(result);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Results.Any(e => e.Id == id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Teams = _context.Teams
                .Select(t => new SelectListItem
                {
                    Value = t.Id.ToString(),
                    Text = t.Name
                })
                .ToList();
            return View(result);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var result = await _context.Results
                .Include(r => r.HomeTeam)
                .Include(r => r.AwayTeam)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (result == null) return NotFound();

            return View(result);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var result = await _context.Results
                .Include(r => r.HomeTeam)
                .Include(r => r.AwayTeam)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (result == null) return NotFound();

            return View(result);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var result = await _context.Results.FindAsync(id);
            if (result != null)
            {
                _context.Results.Remove(result);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
