namespace HospitalManagementSystem.Core.Entities
{
    public class SystemConfiguration
    {
        public required string Key { get; set; }
        public required string Value { get; set; }
        public string? Description { get; set; }
    }
}


