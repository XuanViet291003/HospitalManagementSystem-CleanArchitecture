using HospitalManagementSystem.Application.DTOs.Appointments;

namespace HospitalManagementSystem.Application.DTOs.MedicalRecords
{
    public class MedicalRecordDto
    {
        public long Id { get; set; }
        public long AppointmentId { get; set; }
        public DateTime AppointmentTime { get; set; }
        public string? Symptoms { get; set; }
        public string? Diagnosis { get; set; }
        public string? TreatmentPlan { get; set; }
        public DateTime? FollowUpDate { get; set; }
        public required DoctorInfoDto Doctor { get; set; }
        public PatientInfoDto? Patient { get; set; }
    }
}

