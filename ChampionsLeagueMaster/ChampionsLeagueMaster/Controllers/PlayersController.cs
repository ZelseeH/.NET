using ChampionsLeagueMaster.Models;
using ChampionsLeagueMaster.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace ChampionsLeagueMaster.Controllers
{
    public class PlayersController : Controller
    {
        private readonly IPlayerRepository _playerRepository;
        private readonly ITeamRepository _teamRepository;

        public PlayersController(IPlayerRepository playerRepository, ITeamRepository teamRepository)
        {
            _playerRepository = playerRepository;
            _teamRepository = teamRepository;
        }

        public async Task<IActionResult> Index(string teamName, string position, string sortOrder)
        {
            var players = await _playerRepository.GetAllAsync();

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

            return View(await players.ToListAsync());
        }

        public async Task<IActionResult> Create()
        {
            var teams = await _teamRepository.GetAllAsync();
            ViewData["Teams"] = new SelectList(await teams.ToListAsync(), "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Player player)
        {
            if (ModelState.IsValid)
            {
                await _playerRepository.InsertAsync(player);
                await _playerRepository.SaveAsync();
                return RedirectToAction(nameof(Index));
            }

            var teams = await _teamRepository.GetAllAsync();
            ViewData["Teams"] = new SelectList(await teams.ToListAsync(), "Id", "Name");
            return View(player);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var player = await _playerRepository.GetByIdAsync(id.Value);
            if (player == null)
                return NotFound();

            var teams = await _teamRepository.GetAllAsync();
            ViewData["Teams"] = new SelectList(await teams.ToListAsync(), "Id", "Name", player.TeamId);
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
                await _playerRepository.UpdateAsync(player);
                await _playerRepository.SaveAsync();
                return RedirectToAction(nameof(Index));
            }

            var teams = await _teamRepository.GetAllAsync();
            ViewData["Teams"] = new SelectList(await teams.ToListAsync(), "Id", "Name", player.TeamId);
            return View(player);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var player = await _playerRepository.GetByIdAsync(id.Value);
            if (player == null)
                return NotFound();

            return View(player);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var player = await _playerRepository.GetByIdAsync(id.Value);
            if (player == null)
                return NotFound();

            return View(player);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _playerRepository.DeleteAsync(id);
            await _playerRepository.SaveAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
