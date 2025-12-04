namespace HospitalManagementSystem.Application.DTOs.Appointments
{
    public class CreateAppointmentDto
    {
        public long DoctorId { get; set; }
        public required DateTime AppointmentTime { get; set; }
        public int DurationMinutes { get; set; } = 30; // Default 30 minutes
        public string? Notes { get; set; }
    }
}


