﻿using System.ComponentModel.DataAnnotations;

namespace DndMassCombat.Models.Simulation
{
    public class GroupViewModel
    {
        [Display(Name = "Hit point")]
        public int HitPoint { get; set; }
        
        [Display(Name = "Unit count")]
        public int UnitCount { get; set; }
    }
}