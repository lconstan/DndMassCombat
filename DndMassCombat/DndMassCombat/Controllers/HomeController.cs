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
            var model = new SimulationViewModel()
            {
                Group1 = new GroupViewModel()
                {
                    HitPoint = 100,
                    UnitCount = 20
                },
                Group2 = new GroupViewModel()
                {
                    HitPoint = 100,
                    UnitCount = 20
                },
                UnitDescription1 = new UnitDescriptionViewModel()
                {
                    Name = "HobGob",
                    ArmorClass = 17,
                    DamageBonus = 1,
                    DamageDice = Dice.D6,
                    HitBonus = 2,
                    HitPoint = 5
                },
                UnitDescription2 = new UnitDescriptionViewModel()
                {
                    Name = "HobGob",
                    ArmorClass = 17,
                    DamageBonus = 1,
                    DamageDice = Dice.D6,
                    HitBonus = 2,
                    HitPoint = 5
                },
            };
            return View(model);
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