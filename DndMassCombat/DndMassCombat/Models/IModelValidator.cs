using DndMassCombat.Models.ViewModels;

namespace DndMassCombat.Models
{
    public interface IModelValidator
    {
        bool IsModelValid(SimulationViewModel simulationViewModel);
    }
}