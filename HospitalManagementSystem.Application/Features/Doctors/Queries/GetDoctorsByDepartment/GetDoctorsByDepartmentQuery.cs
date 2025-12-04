using HospitalManagementSystem.Application.DTOs.Doctors;
using MediatR;

namespace HospitalManagementSystem.Application.Features.Doctors.Queries.GetDoctorsByDepartment
{
    public class GetDoctorsByDepartmentQuery : IRequest<List<DoctorDto>>
    {
        public required long DepartmentId { get; set; }
    }
}


