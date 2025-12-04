namespace HospitalManagementSystem.Core.Entities
{
    public class Doctor : BaseEntity
    {
        public long UserId { get; set; }
        public User? User { get; set; }
        public long DepartmentId { get; set; }
        public Department? Department { get; set; }
        public required string Specialization { get; set; }
        public required string LicenseNumber { get; set; }
        public required decimal ConsultationFee { get; set; }
    }
}


