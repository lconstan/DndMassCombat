using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DndMassCombat.Models.ViewModels
{
    public class GroupViewModel
    {
        [Display(Name = "Hit point")]
        [Range(0, 100_000_000)]
        [Required]
        public int HitPoint { get; set; }
        
        [Display(Name = "Unit count")]
        [Range(0, 1_000_000)]
        [Required]
        public int UnitCount { get; set; }

        public string UnitsHpJson { get; set; }
    }
}