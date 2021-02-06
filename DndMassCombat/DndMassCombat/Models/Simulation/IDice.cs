using DndMassCombat.Models.ViewModels;

namespace DndMassCombat.Models.Simulation
{
    public interface IDice
    {
        int Roll(DamageDice damageDice);
    }
}