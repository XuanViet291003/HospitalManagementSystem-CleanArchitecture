using MediatR;

namespace HospitalManagementSystem.Application.Features.Doctors.Commands.CreateDoctor
{
    public class CreateDoctorCommand : IRequest<long>
    {
        public long UserId { get; set; }
        public long DepartmentId { get; set; }
        public required string Specialization { get; set; }
        public required string LicenseNumber { get; set; }
        public decimal ConsultationFee { get; set; }
    }
}

