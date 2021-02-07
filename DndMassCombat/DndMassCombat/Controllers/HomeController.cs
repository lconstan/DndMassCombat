using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using DndMassCombat.Models;
using DndMassCombat.Models.Simulation;
using DndMassCombat.Models.ViewModels;

namespace DndMassCombat.Controllers
{
    public class HomeController : Controller
    {
        private readonly ISimulationRunner _simulationRunner;

        public HomeController(ISimulationRunner simulationRunner)
        {
            _simulationRunner = simulationRunner;
        }

        public IActionResult Index()
        {
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Simulate(SimulationViewModel simulationViewModel)
        {
            if (ModelState.IsValid)
            {
                _simulationRunner.Simulate(simulationViewModel);
            }

            ModelState.Clear();
            simulationViewModel.UnitDescription1.IsAttacking = false;
            simulationViewModel.UnitDescription2.IsAttacking = false;

            return View("Index", simulationViewModel);
        }

        public IActionResult Details()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }
    }
}