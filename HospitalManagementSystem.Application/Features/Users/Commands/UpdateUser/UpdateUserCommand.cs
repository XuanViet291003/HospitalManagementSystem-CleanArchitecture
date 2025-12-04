using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace HospitalManagementSystem.Application.Features.Users.Commands.UpdateUser
{
    public class UpdateUserCommand : IRequest
    {
        public long Id { get; set; }
        public required string Email { get; set; }
        public bool IsActive { get; set; }
        public long RoleId { get; set; }
    }
}
