namespace HospitalManagementSystem.Core.Entities
{
    public class Department : BaseEntity
    {
        public required string Name { get; set; } 
        public string? Description { get; set; }
    }
}
