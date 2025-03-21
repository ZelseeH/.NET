using ChampionsLeagueMaster.Models;
using ChampionsLeagueMaster.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ChampionsLeagueMaster.Controllers
{
    public class TeamsController : Controller
    {
        private readonly ITeamRepository _teamRepository;

        public TeamsController(ITeamRepository teamRepository)
        {
            _teamRepository = teamRepository;
        }

        // GET: Teams
        public async Task<IActionResult> Index()
        {
            var teams = await _teamRepository.GetAllAsync();
            return View(teams);
        }

        // GET: Teams/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Teams/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Team team)
        {
            if (ModelState.IsValid)
            {
                await _teamRepository.InsertAsync(team);
                await _teamRepository.SaveAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(team);
        }

        // GET: Teams/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var team = await _teamRepository.GetByIdAsync(id.Value);
            if (team == null) return NotFound();

            return View(team);
        }

        // GET: Teams/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var team = await _teamRepository.GetByIdAsync(id.Value);
            if (team == null) return NotFound();

            return View(team);
        }

        // POST: Teams/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Team team)
        {
            if (id != team.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    await _teamRepository.UpdateAsync(team);
                    await _teamRepository.SaveAsync();
                }
                catch
                {
                    if (!await _teamRepository.ExistsAsync(team.Id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(team);
        }

        // GET: Teams/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var team = await _teamRepository.GetByIdAsync(id.Value);
            if (team == null) return NotFound();

            return View(team);
        }

        // POST: Teams/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _teamRepository.DeleteAsync(id);
            await _teamRepository.SaveAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
