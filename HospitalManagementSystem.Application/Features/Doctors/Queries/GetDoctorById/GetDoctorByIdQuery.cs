using MediatR;
using HospitalManagementSystem.Application.DTOs.Doctors;

namespace HospitalManagementSystem.Application.Features.Doctors.Queries.GetDoctorById
{
    public class GetDoctorByIdQuery : IRequest<DoctorDto>
    {
        public long Id { get; set; }
    }
}
