using DndMassCombat.Models.ViewModels;

namespace DndMassCombat.Models
{
    public interface ISimulationRunner
    {
        void Simulate(SimulationViewModel simulationViewModel);
    }
}