using ChampionsLeagueMaster.Models;
using ChampionsLeagueMaster.Services;
using ChampionsLeagueMaster.ViewModels.Players;
using Microsoft.AspNetCore.Mvc;

namespace ChampionsLeagueMaster.Controllers
{
    public class PlayersController : Controller
    {
        private readonly IPlayerService _playerService;

        public PlayersController(IPlayerService playerService)
        {
            _playerService = playerService;
        }
        public async Task<IActionResult> Index(string teamName, string position, string sortOrder, int pageIndex = 1, int pageSize = 10)
        {
            var paginatedPlayers = await _playerService.GetPaginatedPlayerViewModelsAsync(teamName, position, sortOrder, pageIndex, pageSize);

            var viewModel = new PlayerListViewModel
            {
                Players = paginatedPlayers,
                CurrentTeamFilter = teamName,
                CurrentPositionFilter = position,
                CurrentSortOrder = sortOrder,
                AvailablePositions = await _playerService.GetAvailablePositionsAsync()
            };

            return View(viewModel);
        }



        public async Task<IActionResult> Create()
        {
            var viewModel = new PlayerCreateEditViewModel
            {
                Teams = await _playerService.GetTeamsSelectListAsync()
            };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PlayerCreateEditViewModel playerViewModel)
        {
            if (ModelState.IsValid)
            {
                await _playerService.CreatePlayerAsync(playerViewModel);
                return RedirectToAction(nameof(Index));
            }

            playerViewModel.Teams = await _playerService.GetTeamsSelectListAsync(playerViewModel.TeamId);
            return View(playerViewModel);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var playerViewModel = await _playerService.GetPlayerForEditAsync(id.Value);
            if (playerViewModel == null)
                return NotFound();

            return View(playerViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, PlayerCreateEditViewModel playerViewModel)
        {
            if (id != playerViewModel.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                await _playerService.UpdatePlayerAsync(playerViewModel);
                return RedirectToAction(nameof(Index));
            }

            playerViewModel.Teams = await _playerService.GetTeamsSelectListAsync(playerViewModel.TeamId);
            return View(playerViewModel);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var playerViewModel = await _playerService.GetPlayerViewModelByIdAsync(id.Value);
            if (playerViewModel == null)
                return NotFound();

            return View(playerViewModel);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var playerViewModel = await _playerService.GetPlayerViewModelByIdAsync(id.Value);
            if (playerViewModel == null)
                return NotFound();

            return View(playerViewModel);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _playerService.DeletePlayerAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
