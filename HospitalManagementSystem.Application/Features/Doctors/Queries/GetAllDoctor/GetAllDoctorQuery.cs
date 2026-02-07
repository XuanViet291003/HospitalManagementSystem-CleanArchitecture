using HospitalManagementSystem.Application.DTOs.Doctors;
using MediatR;

namespace HospitalManagementSystem.Application.Features.Doctors.Queries.GetAllDoctor
{
    public class GetAllDoctorQuery : IRequest<List<DoctorDto>>
    {

    }
}
