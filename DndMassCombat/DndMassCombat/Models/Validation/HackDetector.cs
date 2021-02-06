using DndMassCombat.Models.ViewModels;

namespace DndMassCombat.Models.Validation
{
    public class HackDetector : IHackDetector
    {
        public bool IsModelValid(SimulationViewModel simulationViewModel)
        {
            if (simulationViewModel.UnitDescription1.IsAttacking == simulationViewModel.UnitDescription2.IsAttacking)
            {
                return false;
            }
            
            return true;
        }
    }
}