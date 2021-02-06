using DndMassCombat.Models.ViewModels;

namespace DndMassCombat.Models.Simulation
{
    public interface ISimulationRunner
    {
        void Simulate(SimulationViewModel simulationViewModel);
    }
}