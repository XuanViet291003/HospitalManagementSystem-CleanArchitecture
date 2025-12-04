namespace HospitalManagementSystem.Core.Entities
{
    public class Appointment : BaseEntity
    {
        public long PatientId { get; set; }
        public Patient? Patient { get; set; }
        public long DoctorId { get; set; }
        public Doctor? Doctor { get; set; }
        public required DateTime AppointmentTime { get; set; }
        public required int DurationMinutes { get; set; }
        public required byte Status { get; set; } // 0:Scheduled, 1:Completed, 2:Cancelled, 3:CheckedIn, 4:NoShow
        public string? Notes { get; set; }
    }
}


