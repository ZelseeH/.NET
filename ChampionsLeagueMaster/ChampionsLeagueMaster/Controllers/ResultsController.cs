using ChampionsLeagueMaster.Models;
using ChampionsLeagueMaster.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ChampionsLeagueMaster.Controllers
{
    public class ResultsController : Controller
    {
        private readonly IResultRepository _resultRepository;
        private readonly ITeamRepository _teamRepository; 

        public ResultsController(IResultRepository resultRepository, ITeamRepository teamRepository)
        {
            _resultRepository = resultRepository;
            _teamRepository = teamRepository; // Inject ITeamRepository
        }

        public async Task<IActionResult> Index(string season, string round)
        {
            var seasons = await _resultRepository.GetSeasonsAsync();
            var rounds = await _resultRepository.GetRoundsAsync();

            ViewBag.Seasons = seasons;
            ViewBag.Rounds = rounds;
            ViewBag.SelectedSeason = season ?? seasons.FirstOrDefault();
            ViewBag.SelectedRound = round ?? rounds.FirstOrDefault();

            var results = await _resultRepository.GetAllAsync();

            results = results.Where(r => r.Season == (season ?? seasons.FirstOrDefault()))
                             .Where(r => r.Round == (round ?? rounds.FirstOrDefault()));

            return View(await results.ToListAsync());
        }

        public async Task<IActionResult> Create()
        {
            var teams = await _teamRepository.GetAllAsync();
            ViewBag.Teams = teams; // Pass teams directly, as the view expects a collection
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("HomeTeamId,AwayTeamId,Season,HomeTeamGoals,AwayTeamGoals,MatchDay,MatchTime,Status,Round")] Result result)
        {
            if (ModelState.IsValid)
            {
                await _resultRepository.InsertAsync(result);
                await _resultRepository.SaveAsync();
                return RedirectToAction(nameof(Index));
            }
            var teams = await _teamRepository.GetAllAsync();
            ViewBag.Teams = teams; // Pass teams again if validation fails
            return View(result);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var result = await _resultRepository.GetByIdAsync(id.Value);
            if (result == null) return NotFound();

            var teams = await _teamRepository.GetAllAsync();
            ViewBag.Teams = teams.Select(t => new SelectListItem
            {
                Value = t.Id.ToString(),
                Text = t.Name ?? "Unknown Team"
            }).ToList();
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
                    await _resultRepository.UpdateAsync(result);
                    await _resultRepository.SaveAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (await _resultRepository.GetByIdAsync(id) == null) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }

            var teams = await _teamRepository.GetAllAsync();
            ViewBag.Teams = teams.Select(t => new SelectListItem
            {
                Value = t.Id.ToString(),
                Text = t.Name ?? "Unknown Team"
            }).ToList();
            return View(result);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var result = await _resultRepository.GetByIdAsync(id.Value);
            if (result == null) return NotFound();

            return View(result);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var result = await _resultRepository.GetByIdAsync(id.Value);
            if (result == null) return NotFound();

            return View(result);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _resultRepository.DeleteAsync(id);
            await _resultRepository.SaveAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}