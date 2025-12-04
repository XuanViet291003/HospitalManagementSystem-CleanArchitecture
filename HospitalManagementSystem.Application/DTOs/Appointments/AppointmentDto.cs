namespace HospitalManagementSystem.Application.DTOs.Appointments
{
    public class AppointmentDto
    {
        public long Id { get; set; }
        public required DateTime AppointmentTime { get; set; }
        public required int DurationMinutes { get; set; }
        public required byte Status { get; set; }
        public string? Notes { get; set; }
        public required DoctorInfoDto Doctor { get; set; }
        public PatientInfoDto? Patient { get; set; }
    }

    public class DoctorInfoDto
    {
        public long Id { get; set; }
        public required string FullName { get; set; }
        public required string Specialization { get; set; }
        public required string DepartmentName { get; set; }
    }

    public class PatientInfoDto
    {
        public long Id { get; set; }
        public required string PatientCode { get; set; }
        public string? FullName { get; set; }
    }
}


