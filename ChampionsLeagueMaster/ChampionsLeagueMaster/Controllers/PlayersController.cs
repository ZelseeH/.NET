using ChampionsLeagueMaster.Models;
using ChampionsLeagueMaster.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ChampionsLeagueMaster.Repository;

namespace ChampionsLeagueMaster.Controllers
{
    public class PlayersController : Controller
    {
        private readonly IPlayerService _playerService;

        public PlayersController(IPlayerService playerService)
        {
            _playerService = playerService;
        }

        public async Task<IActionResult> Index(string teamName, string position, string sortOrder)
        {
        
            ViewData["CurrentTeamFilter"] = teamName;
            ViewData["CurrentPositionFilter"] = position;
            ViewData["CurrentSortOrder"] = sortOrder;

            ViewData["AvailablePositions"] = await _playerService.GetAvailablePositionsAsync();

            var playersQuery = await _playerService.GetAllPlayersAsync(teamName, position, sortOrder);
            var players = await playersQuery.ToListAsync();
            return View(players);
        }


        public async Task<IActionResult> Create()
        {
            ViewData["Teams"] = await _playerService.GetTeamsSelectListAsync();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Player player)
        {
            if (ModelState.IsValid)
            {
                await _playerService.CreatePlayerAsync(player);
                return RedirectToAction(nameof(Index));
            }

            ViewData["Teams"] = await _playerService.GetTeamsSelectListAsync(player.TeamId);
            return View(player);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var player = await _playerService.GetPlayerByIdAsync(id.Value);
            if (player == null)
                return NotFound();

            ViewData["Teams"] = await _playerService.GetTeamsSelectListAsync(player.TeamId);
            return View(player);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Player player)
        {
            if (id != player.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                await _playerService.UpdatePlayerAsync(player);
                return RedirectToAction(nameof(Index));
            }

            ViewData["Teams"] = await _playerService.GetTeamsSelectListAsync(player.TeamId);
            return View(player);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var player = await _playerService.GetPlayerByIdAsync(id.Value);
            if (player == null)
                return NotFound();

            return View(player);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var player = await _playerService.GetPlayerByIdAsync(id.Value);
            if (player == null)
                return NotFound();

            return View(player);
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
