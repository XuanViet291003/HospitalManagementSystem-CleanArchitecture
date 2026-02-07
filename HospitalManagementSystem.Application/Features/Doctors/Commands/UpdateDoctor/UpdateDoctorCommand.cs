using MediatR;
using HospitalManagementSystem.Application.DTOs.Doctors;

namespace HospitalManagementSystem.Application.Features.Doctors.Commands.UpdateDoctor
{
    public class UpdateDoctorCommand : IRequest<DoctorDto>
    {
        public long Id { get; set; }
        public string? Specialization { get; set; }
        public string? LicenseNumber { get; set; }
        public decimal? ConsultationFee { get; set; }
        public long? DepartmentId { get; set; }
    }
}