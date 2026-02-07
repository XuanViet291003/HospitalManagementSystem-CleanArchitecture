using HospitalManagementSystem.Core.Entities;
using MediatR;

namespace HospitalManagementSystem.Application.Features.DoctorSchedules.Queries.GetDoctorScheduleById
{
    public class GetDoctorScheduleByIdQuery : IRequest<DoctorSchedule>
    {
        public long Id { get; set; }
    }
}

