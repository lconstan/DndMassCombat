using System.ComponentModel.DataAnnotations;

namespace DndMassCombat.Models.Simulation
{
    public class GroupViewModel
    {
        public string Name { get; set; }
        
        [Display(Name = "Hit point")]
        public int HitPoint { get; set; }
        
        [Display(Name = "Armor class")]
        public int ArmorClass { get; set; }
        
        [Display(Name = "Hit bonus")]
        public int HitBonus { get; set; }
        public Damage Damage { get; set; }
        
        [Display(Name = "Damage bonus")]
        
        public int DamageBonus { get; set; }
    }
}