using DndMassCombat.Models.ViewModels;

namespace DndMassCombat.Models
{
    public interface IHackDetector
    {
        bool IsModelValid(SimulationViewModel simulationViewModel);
    }
}