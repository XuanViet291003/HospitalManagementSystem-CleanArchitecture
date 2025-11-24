using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using HospitalManagementSystem.Application.DTOs.Users;  

namespace HospitalManagementSystem.Application.Features.Users.Queries.Login
{
    public class LoginQuery : IRequest<LoginResponseDto>
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
