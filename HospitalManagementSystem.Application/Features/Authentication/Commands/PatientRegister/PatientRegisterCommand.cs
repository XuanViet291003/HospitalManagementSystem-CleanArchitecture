using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace HospitalManagementSystem.Application.Features.Authentication.Commands.PatientRegister
{
    public class PatientRegisterCommand : IRequest<long>
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
        public required string FullName { get; set; }
    }
}


