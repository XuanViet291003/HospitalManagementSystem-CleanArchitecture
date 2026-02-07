using MediatR;
using HospitalManagementSystem.Application.DTOs.Departments;

namespace HospitalManagementSystem.Application.Features.Departments.Queries.GetByName
{
    public class GetByNameDepartment : IRequest<DepartmentDto>
    {
        public string Name { get; set; }
    }
}
