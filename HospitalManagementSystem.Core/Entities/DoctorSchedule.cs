namespace HospitalManagementSystem.Core.Entities
{
    public class DoctorSchedule : BaseEntity
    {
        public long DoctorId { get; set; }
        public Doctor? Doctor { get; set; }
        public required DateTime WorkDate { get; set; }
        public required TimeSpan StartTime { get; set; }
        public required TimeSpan EndTime { get; set; }
        public required int SlotDurationMinutes { get; set; }
        public bool IsAvailable { get; set; } = true;
    }
}


