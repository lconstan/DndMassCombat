using DndMassCombat.Models.ViewModels;

namespace DndMassCombat.Models.Simulation
{
    public interface IDiceRoller
    {
        int Roll(Dice dice);
    }
}