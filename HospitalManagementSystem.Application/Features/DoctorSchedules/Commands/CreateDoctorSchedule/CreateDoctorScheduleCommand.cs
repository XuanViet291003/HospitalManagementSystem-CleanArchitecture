using MediatR;

namespace HospitalManagementSystem.Application.Features.DoctorSchedules.Commands.CreateDoctorSchedule
{
    public class CreateDoctorScheduleCommand : IRequest<long>
    {
        public long DoctorId { get; set; }
        public required DateTime WorkDate { get; set; }
        public required TimeSpan StartTime { get; set; }
        public required TimeSpan EndTime { get; set; }
        public int SlotDurationMinutes { get; set; } = 30;
        public bool IsAvailable { get; set; } = true;
    }
}

