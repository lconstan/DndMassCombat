using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using DndMassCombat.Models;
using DndMassCombat.Models.ViewModels;

namespace DndMassCombat.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHackDetector _hackDetector;
        private readonly ISimulationRunner _simulationRunner;

        public HomeController(IHackDetector hackDetector, ISimulationRunner simulationRunner)
        {
            _hackDetector = hackDetector;
            _simulationRunner = simulationRunner;
        }

        public IActionResult Index()
        {
            return View(new SimulationViewModel());
        }
        
        [HttpPost]
        public IActionResult Simulate(SimulationViewModel simulationViewModel)
        {
            if (ModelState.IsValid)
            {
                if (!_hackDetector.IsModelValid(simulationViewModel))
                {
                    return StatusCode(418);
                }
            
                _simulationRunner.Simulate(simulationViewModel);    
            }

            simulationViewModel.UnitDescription1.IsAttacking = null;
            simulationViewModel.UnitDescription2.IsAttacking = null;
            
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