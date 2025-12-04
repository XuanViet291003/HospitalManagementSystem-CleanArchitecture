namespace HospitalManagementSystem.Core.Entities
{
    public class Patient : BaseEntity
    {
        public long? UserId { get; set; }
        public User? User { get; set; }
        public required string PatientCode { get; set; }
        public string? InsuranceNumber { get; set; }
        public string? MedicalHistory { get; set; }
    }
}


