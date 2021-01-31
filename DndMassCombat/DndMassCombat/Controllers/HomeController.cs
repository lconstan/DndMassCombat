using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using DndMassCombat.Models;
using DndMassCombat.Models.ViewModels;

namespace DndMassCombat.Controllers
{
    public class HomeController : Controller
    {
        private readonly IModelValidator _modelValidator;
        private readonly ISimulationRunner _simulationRunner;

        public HomeController(IModelValidator modelValidator, ISimulationRunner simulationRunner)
        {
            _modelValidator = modelValidator;
            _simulationRunner = simulationRunner;
        }

        public IActionResult Index()
        {
            return View(new SimulationViewModel());
        }
        
        [HttpPost]
        public IActionResult Simulate(SimulationViewModel simulationViewModel)
        {
            if (!_modelValidator.IsModelValid(simulationViewModel))
            {
                return StatusCode(418);
            }
            
            _simulationRunner.Simulate(simulationViewModel);
            
            return View("Index", simulationViewModel);
        }

        public IActionResult Privacy()
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