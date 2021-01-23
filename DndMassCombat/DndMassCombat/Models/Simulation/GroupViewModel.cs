namespace DndMassCombat.Models.Simulation
{
    public class GroupViewModel
    {
        public string Name { get; set; }
        public int HitPoint { get; set; }
        public int ArmorClass { get; set; }
        public int HitBonus { get; set; }
        public Damage Damage { get; set; }
    }
}