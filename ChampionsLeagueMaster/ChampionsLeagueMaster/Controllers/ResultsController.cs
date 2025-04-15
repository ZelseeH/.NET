using ChampionsLeagueMaster.Services;
using ChampionsLeagueMaster.ViewModels.Results;
using Microsoft.AspNetCore.Mvc;
using FluentValidation;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChampionsLeagueMaster.Controllers
{
    public class ResultsController : Controller
    {
        private readonly IResultService _resultService;
        private readonly IValidator<ResultCreateEditViewModel> _validator;

        public ResultsController(IResultService resultService, IValidator<ResultCreateEditViewModel> validator)
        {
            _resultService = resultService;
            _validator = validator;
        }

        public async Task<IActionResult> Index(string season, string round)
        {
            var seasons = await _resultService.GetSeasonsAsync();
            var selectedSeason = season ?? seasons.FirstOrDefault();

            var rounds = await _resultService.GetRoundsBySeasonAsync(selectedSeason);
            var selectedRound = string.IsNullOrEmpty(round) || !rounds.Contains(round)
                ? rounds.FirstOrDefault()
                : round;

            var results = await _resultService.GetFilteredResultsAsync(selectedSeason, selectedRound);

            var viewModel = new ResultListViewModel
            {
                Results = results,
                Seasons = seasons,
                Rounds = rounds,
                SelectedSeason = selectedSeason,
                SelectedRound = selectedRound
            };

            return View(viewModel);
        }

        [HttpGet]
        public async Task<JsonResult> GetRoundsBySeason(string season)
        {
            var rounds = await _resultService.GetRoundsBySeasonAsync(season);
            return Json(rounds);
        }

        public async Task<IActionResult> Create()
        {
            var viewModel = new ResultCreateEditViewModel
            {
                Teams = await _resultService.GetTeamsSelectListAsync()
            };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ResultCreateEditViewModel viewModel)
        {
            var validationResult = await _validator.ValidateAsync(viewModel);

            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
            }

            if (ModelState.IsValid)
            {
                await _resultService.CreateResultAsync(viewModel);
                return RedirectToAction(nameof(Index), new { season = viewModel.Season });
            }

            viewModel.Teams = await _resultService.GetTeamsSelectListAsync();
            return View(viewModel);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var viewModel = await _resultService.GetResultForEditAsync(id.Value);
            if (viewModel == null) return NotFound();

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ResultCreateEditViewModel viewModel)
        {
            if (id != viewModel.Id) return NotFound();

            var validationResult = await _validator.ValidateAsync(viewModel);

            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _resultService.UpdateResultAsync(viewModel);
                }
                catch (KeyNotFoundException)
                {
                    return NotFound();
                }

                return RedirectToAction(nameof(Index), new { season = viewModel.Season });
            }

            viewModel.Teams = await _resultService.GetTeamsSelectListAsync();
            return View(viewModel);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var viewModel = await _resultService.GetResultViewModelByIdAsync(id.Value);
            if (viewModel == null) return NotFound();

            return View(viewModel);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var viewModel = await _resultService.GetResultViewModelByIdAsync(id.Value);
            if (viewModel == null) return NotFound();

            return View(viewModel);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var result = await _resultService.GetResultByIdAsync(id);
            string season = result?.Season;

            await _resultService.DeleteResultAsync(id);
            return RedirectToAction(nameof(Index), new { season });
        }
    }
}
