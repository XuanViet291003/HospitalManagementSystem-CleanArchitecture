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
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
