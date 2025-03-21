using ChampionsLeagueMaster.Models;
using ChampionsLeagueMaster.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ChampionsLeagueMaster.Controllers
{
    public class ResultsController : Controller
    {
        private readonly IResultService _resultService;

        public ResultsController(IResultService resultService)
        {
            _resultService = resultService;
        }

        public async Task<IActionResult> Index(string season, string round)
        {
            var seasons = await _resultService.GetSeasonsAsync();
            var rounds = await _resultService.GetRoundsAsync();

            ViewBag.Seasons = seasons;
            ViewBag.Rounds = rounds;
            ViewBag.SelectedSeason = season ?? seasons.FirstOrDefault();
            ViewBag.SelectedRound = round ?? rounds.FirstOrDefault();

            var results = await _resultService.GetAllResultsAsync(season, round);
            return View(await results.ToListAsync());
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.Teams = await _resultService.GetTeamsSelectListAsync();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("HomeTeamId,AwayTeamId,Season,HomeTeamGoals,AwayTeamGoals,MatchDay,MatchTime,Status,Round")] Result result)
        {
            if (ModelState.IsValid)
            {
                await _resultService.CreateResultAsync(result);
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Teams = await _resultService.GetTeamsSelectListAsync();
            return View(result);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var result = await _resultService.GetResultByIdAsync(id.Value);
            if (result == null) return NotFound();

            ViewBag.Teams = await _resultService.GetTeamsSelectListAsync();
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
                    await _resultService.UpdateResultAsync(result);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (await _resultService.GetResultByIdAsync(id) == null) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Teams = await _resultService.GetTeamsSelectListAsync();
            return View(result);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var result = await _resultService.GetResultByIdAsync(id.Value);
            if (result == null) return NotFound();

            return View(result);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var result = await _resultService.GetResultByIdAsync(id.Value);
            if (result == null) return NotFound();

            return View(result);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _resultService.DeleteResultAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
