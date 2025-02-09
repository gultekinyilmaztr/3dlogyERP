namespace _3dlogyERP.Core.Entities
{
    public class EquipmentType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }

        // Navigation property
        public virtual ICollection<Equipment> Equipments { get; set; }
    }
}
