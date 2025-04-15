using ChampionsLeagueMaster.Services;
using ChampionsLeagueMaster.ViewModels.Teams;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ChampionsLeagueMaster.Controllers
{
    public class TeamsController : Controller
    {
        private readonly ITeamService _teamService;
        private readonly IValidator<TeamCreateEditViewModel> _validator;

        public TeamsController(ITeamService teamService, IValidator<TeamCreateEditViewModel> validator)
        {
            _teamService = teamService;
            _validator = validator;
        }

        public async Task<IActionResult> Index(string countryFilter, string sortOrder, int? pageNumber)
        {
            int pageSize = 10;
            int pageIndex = pageNumber ?? 1;

            var teams = await _teamService.GetPaginatedTeamsAsync(countryFilter, sortOrder, pageIndex, pageSize);
            var availableCountries = await _teamService.GetAvailableCountriesAsync();

            var viewModel = new TeamListViewModel
            {
                Teams = teams,
                CurrentCountryFilter = countryFilter,
                CurrentSortOrder = sortOrder,
                AvailableCountries = availableCountries
            };

            return View(viewModel);
        }

        public IActionResult Create()
        {
            return View(new TeamCreateEditViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TeamCreateEditViewModel viewModel)
        {
            ValidationResult validationResult = await _validator.ValidateAsync(viewModel);

            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }
            }

            if (ModelState.IsValid)
            {
                await _teamService.CreateTeamAsync(viewModel);
                return RedirectToAction(nameof(Index));
            }

            return View(viewModel);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var team = await _teamService.GetTeamViewModelAsync(id.Value);
            if (team == null) return NotFound();

            return View(team);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var team = await _teamService.GetTeamViewModelAsync(id.Value);
            if (team == null) return NotFound();

            var viewModel = new TeamCreateEditViewModel
            {
                Id = team.Id,
                Name = team.Name,
                Country = team.Country,
                FoundedAt = team.FoundedAt
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, TeamCreateEditViewModel viewModel)
        {
            if (id != viewModel.Id) return NotFound();

            ValidationResult validationResult = await _validator.ValidateAsync(viewModel);

            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _teamService.UpdateTeamAsync(viewModel);
                }
                catch
                {
                    if (!await _teamService.TeamExistsAsync(viewModel.Id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }

            return View(viewModel);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var team = await _teamService.GetTeamViewModelAsync(id.Value);
            if (team == null) return NotFound();

            return View(team);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _teamService.DeleteTeamAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
