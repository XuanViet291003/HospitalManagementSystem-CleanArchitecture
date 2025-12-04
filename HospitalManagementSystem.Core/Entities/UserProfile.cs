namespace HospitalManagementSystem.Core.Entities
{
    public class UserProfile : BaseEntity
    {
        public long UserId { get; set; }
        public User? User { get; set; }
        public required string FullName { get; set; }
        public required DateTime DateOfBirth { get; set; }
        public required byte Gender { get; set; } // 0: Male, 1: Female, 2: Other
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        public string? AvatarUrl { get; set; }
    }
}


