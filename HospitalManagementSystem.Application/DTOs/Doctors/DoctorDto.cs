namespace HospitalManagementSystem.Application.DTOs.Doctors
{
    public class DoctorDto
    {
        public long Id { get; set; }
        public required string FullName { get; set; }
        public required string Specialization { get; set; }
        public required string LicenseNumber { get; set; }
        public required decimal ConsultationFee { get; set; }
        public long DepartmentId { get; set; }
        public required string DepartmentName { get; set; }
    }
}


