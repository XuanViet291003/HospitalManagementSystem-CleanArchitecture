
using MediatR;
using HospitalManagementSystem.Application.DTOs.Departments;

namespace HospitalManagementSystem.Application.Features.Departments.Queries.GetAllDepartments
{
    public class GetAllDepartmentsQuery : IRequest<List<DepartmentDto>>
    {
        
    }
}
