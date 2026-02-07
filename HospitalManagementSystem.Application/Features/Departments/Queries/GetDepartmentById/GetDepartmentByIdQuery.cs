using MediatR;
using HospitalManagementSystem.Application.DTOs.Departments;

namespace HospitalManagementSystem.Application.Features.Departments.Queries.GetDepartmentById
{
    public class GetDepartmentByIdQuery : IRequest<DepartmentDto>
    {
        public long Id { get; set; }
    }
}

