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
        [Range(0, 100)]
        public int HitPoint { get; set; }
        
        [Display(Name = "Armor class")]
        [Required]
        [Range(0, 100)]
        public int ArmorClass { get; set; }
        
        [Display(Name = "Hit bonus")]
        [Required]
        [Range(0, 100)]
        public int HitBonus { get; set; }
        
        [Required]
        public Dice DamageDice { get; set; }
        
        
        [Display(Name = "Damage bonus")]
        [Required]
        [Range(0, 100)]
        public int DamageBonus { get; set; }
        
        public bool? IsAttacking { get; set; }
    }
}