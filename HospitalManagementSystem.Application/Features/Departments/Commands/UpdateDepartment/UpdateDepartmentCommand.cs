using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace HospitalManagementSystem.Application.Features.Departments.Commands.UpdateDepartment
{
    public class UpdateDepartmentCommand : IRequest
    {
        public long Id { get; set; } 
        public required string Name { get; set; }
        public string? Description { get; set; }
    }
}
