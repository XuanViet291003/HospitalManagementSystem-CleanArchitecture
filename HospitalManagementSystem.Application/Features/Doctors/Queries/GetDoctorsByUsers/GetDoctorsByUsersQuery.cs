using MediatR;
using HospitalManagementSystem.Application.DTOs.Doctors;

namespace HospitalManagementSystem.Application.Features.Doctors.Queries.GetDoctorsByUsers
{
    public class GetDoctorsByUsersQuery : IRequest<DoctorDto?>
    {
        public long UserId { get; set; }
    }
}
