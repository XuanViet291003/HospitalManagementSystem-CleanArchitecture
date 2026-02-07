using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HospitalManagementSystem.Core.Entities;
using HospitalManagementSystem.Core.Interfaces.Repositories;
using MediatR;



namespace HospitalManagementSystem.Application.Features.Doctors.Commands.AddDoctors
{
    public class AddDoctorsCommand : IRequest<long>
    {
        public required long UserId { get; set; }
        public required long DepartmentId { get; set; }
        public required string Specialization { get; set; }
        public required string LicenseNumber { get; set; }
        public required decimal ConsultationFee { get; set; }

    }
}
