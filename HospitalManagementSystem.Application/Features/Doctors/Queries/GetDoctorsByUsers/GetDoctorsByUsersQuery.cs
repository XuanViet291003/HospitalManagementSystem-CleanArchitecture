using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using HospitalManagementSystem.Application.DTOs.Doctors;
using HospitalManagementSystem.Application.DTOs.Users;



namespace HospitalManagementSystem.Application.Features.Doctors.Queries.GetDoctorsByUsers
{
    public class GetDoctorsByUsersQuery : IRequest<List<DoctorDto>>
    {
        public long UserId { get; set; }
    }
}
