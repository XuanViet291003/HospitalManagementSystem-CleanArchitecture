namespace HospitalManagementSystem.Application.DTOs.Appointments
{
    public class TimeSlotDto
    {
        public required DateTime StartTime { get; set; }
        public required DateTime EndTime { get; set; }
        public bool IsAvailable { get; set; }
    }
}


