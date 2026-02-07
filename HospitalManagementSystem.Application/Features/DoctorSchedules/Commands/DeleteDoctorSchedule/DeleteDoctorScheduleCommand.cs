using MediatR;

namespace HospitalManagementSystem.Application.Features.DoctorSchedules.Commands.DeleteDoctorSchedule
{
    public class DeleteDoctorScheduleCommand : IRequest
    {
        public long Id { get; set; }
    }
}

