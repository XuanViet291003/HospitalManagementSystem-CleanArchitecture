namespace HospitalManagementSystem.Application.DTOs.SystemConfigurations
{
    public class SystemConfigurationDto
    {
        public required string Key { get; set; }
        public required string Value { get; set; }
        public string? Description { get; set; }
    }
}


