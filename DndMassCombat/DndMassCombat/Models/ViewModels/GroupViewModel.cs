using System.ComponentModel.DataAnnotations;

namespace DndMassCombat.Models.ViewModels
{
    public class GroupViewModel
    {
        [Display(Name = "Hit point")]
        [Required]
        public int HitPoint { get; set; }
        
        [Display(Name = "Unit count")]
        public int UnitCount { get; }
    }
}