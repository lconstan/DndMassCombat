using System;
using DndMassCombat.Models.ViewModels;

namespace DndMassCombat.Models.Simulation
{
    public class Dice : IDice
    {
        private readonly Random _rand = new Random();
        
        public int Roll(DamageDice damageDice)
        {
            var value = (int) damageDice;
            return _rand.Next() % value + 1;
        }
    }
}