namespace HospitalManagementSystem.Application.DTOs.MedicalRecords
{
    public class UpdateMedicalRecordDto
    {
        public string? Symptoms { get; set; }
        public string? Diagnosis { get; set; }
        public string? TreatmentPlan { get; set; }
        public DateTime? FollowUpDate { get; set; }
    }
}


