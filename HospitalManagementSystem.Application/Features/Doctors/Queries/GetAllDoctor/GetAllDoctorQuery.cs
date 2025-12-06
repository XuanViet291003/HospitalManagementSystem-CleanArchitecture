using HospitalManagementSystem.Application.DTOs.Doctors;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagementSystem.Application.Features.Doctors.Queries.GetAllDoctor
{
    public class GetAllDoctorQuery : IRequest<List<DoctorDto>>
    {

    }
}
