using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using HospitalManagementSystem.Application.DTOs.Doctors;

namespace HospitalManagementSystem.Application.Features.Doctors.Queries.GetDoctorById
{
    public class GetDoctorByIdQuery : IRequest<DoctorDto>
    {
        public long Id { get; set; }
    }
}
