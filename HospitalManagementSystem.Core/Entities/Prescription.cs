namespace HospitalManagementSystem.Core.Entities
{
    public class Prescription : BaseEntity
    {
        public long MedicalRecordId { get; set; }
        public MedicalRecord? MedicalRecord { get; set; }
        public required DateTime IssuedDate { get; set; }
    }
}


