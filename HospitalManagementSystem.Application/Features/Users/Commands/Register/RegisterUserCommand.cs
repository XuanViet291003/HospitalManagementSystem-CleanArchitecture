using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace HospitalManagementSystem.Application.Features.Users.Commands.Register
{
    public class RegisterUserCommand : IRequest<long>
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
        public required string Fullname { get; set; }

        public string RoleName { get; set; } = "Patient";
    }
}
