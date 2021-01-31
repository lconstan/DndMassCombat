using DndMassCombat.Models.ViewModels;

namespace DndMassCombat.Models
{
    public class ModelValidator : IModelValidator
    {
        public bool IsModelValid(SimulationViewModel simulationViewModel)
        {
            if (simulationViewModel.Group1 == null
                || simulationViewModel.Group2 == null
                || simulationViewModel.UnitDescription1 == null
                || simulationViewModel.UnitDescription2 == null)
            {
                return false;
            }

            if (simulationViewModel.UnitDescription1.IsAttacking == simulationViewModel.UnitDescription2.IsAttacking)
            {
                return false;
            }

            return true;
        }
    }
}