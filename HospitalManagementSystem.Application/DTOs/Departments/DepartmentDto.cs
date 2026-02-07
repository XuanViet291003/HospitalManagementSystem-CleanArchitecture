namespace HospitalManagementSystem.Application.DTOs.Departments
{
    public class DepartmentDto
    {
        public long Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
    }
}
