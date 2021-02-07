using System;
using DndMassCombat.Models.ViewModels;

namespace DndMassCombat.Models.Simulation
{
    public class DiceRoller : IDiceRoller
    {
        private readonly Random _rand = new Random();
        
        public int Roll(Dice dice)
        {
            var value = (int) dice;
            return _rand.Next() % value + 1;
        }
    }
}