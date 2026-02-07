namespace HospitalManagementSystem.Core.Entities
{
    public class User : BaseEntity
    {
        public required string Email { get; set; }
        public required string PasswordHash { get; set; }
        public bool IsActive { get; set; } = true;
        public long RoleId { get; set; } 
        public Role? Role { get; set; } 
    }
}
