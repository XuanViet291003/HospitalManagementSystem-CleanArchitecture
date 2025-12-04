using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagementSystem.Application.Features.Users.Commands.Register
{
    public class CreateUserCommand : IRequest<long>
    {
        public required string Email { get; set; }
        public required string FullName { get; set; }
        public required string RoleName { get; set; }
    }
}
