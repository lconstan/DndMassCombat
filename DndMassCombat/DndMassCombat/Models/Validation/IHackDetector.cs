using DndMassCombat.Models.ViewModels;

namespace DndMassCombat.Models.Validation
{
    public interface IHackDetector
    {
        bool IsModelValid(SimulationViewModel simulationViewModel);
    }
}