using DndMassCombat.Models.ViewModels;

namespace DndMassCombat.Models.Simulation
{
    public class SimulationRunner : ISimulationRunner
    {
        public void Simulate(SimulationViewModel simulationViewModel)
        {
            if (simulationViewModel.UnitDescription1.IsAttacking == true)
                Simulate(simulationViewModel.UnitDescription1, 
                         simulationViewModel.Group1, 
                         simulationViewModel.UnitDescription2, 
                         simulationViewModel.Group2);
            else
                Simulate(simulationViewModel.UnitDescription2, 
                         simulationViewModel.Group2,
                         simulationViewModel.UnitDescription1,
                         simulationViewModel.Group1);
        }

        private void Simulate(UnitDescriptionViewModel attackingUnit, GroupViewModel attackingGroup, 
                              UnitDescriptionViewModel defendingUnit, GroupViewModel defendingGroup)
        {
            for (int i = 0; i < attackingGroup.UnitCount; i++)
            {
                var targetIndex = i % defendingGroup.UnitCount;
                
                // Roll attack damage
                
            }
        }
    }
}