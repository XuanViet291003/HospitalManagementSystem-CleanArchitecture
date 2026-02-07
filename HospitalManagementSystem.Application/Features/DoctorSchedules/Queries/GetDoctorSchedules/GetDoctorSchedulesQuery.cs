using HospitalManagementSystem.Core.Entities;
using MediatR;

namespace HospitalManagementSystem.Application.Features.DoctorSchedules.Queries.GetDoctorSchedules
{
    public class GetDoctorSchedulesQuery : IRequest<List<DoctorSchedule>>
    {
        public long DoctorId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}

