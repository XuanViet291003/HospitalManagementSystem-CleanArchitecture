namespace HospitalManagementSystem.Core.Entities
{
    public class MedicalRecord : BaseEntity
    {
        public long AppointmentId { get; set; }
        public Appointment? Appointment { get; set; }
        public long PatientId { get; set; }
        public Patient? Patient { get; set; }
        public long DoctorId { get; set; }
        public Doctor? Doctor { get; set; }
        public string? Symptoms { get; set; }
        public string? Diagnosis { get; set; }
        public string? TreatmentPlan { get; set; }
        public DateTime? FollowUpDate { get; set; }
    }
}


