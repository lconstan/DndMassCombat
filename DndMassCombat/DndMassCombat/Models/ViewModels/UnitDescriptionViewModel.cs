using System.ComponentModel.DataAnnotations;

namespace DndMassCombat.Models.ViewModels
{
    public class UnitDescriptionViewModel
    {
        [StringLength(100, ErrorMessage = "Name lenght can't be more than 100")]
        [Required]
        public string Name { get; set; }
        
        [Display(Name = "Hit point")]
        [Required]
        public int HitPoint { get; set; }
        
        [Display(Name = "Armor class")]
        [Required]
        public int ArmorClass { get; set; }
        
        [Display(Name = "Hit bonus")]
        [Required]
        public int HitBonus { get; set; }
        
        [Required]
        public Damage Damage { get; set; }
        
        
        [Display(Name = "Damage bonus")]
        [Required]
        public int DamageBonus { get; set; }
        
        public bool? IsAttacking { get; set; }
    }
}