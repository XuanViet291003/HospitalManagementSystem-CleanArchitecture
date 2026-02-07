using MediatR;

namespace HospitalManagementSystem.Application.Features.DoctorSchedules.Commands.UpdateDoctorSchedule
{
    public class UpdateDoctorScheduleCommand : IRequest
    {
        public long Id { get; set; }
        public DateTime? WorkDate { get; set; }
        public TimeSpan? StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }
        public int? SlotDurationMinutes { get; set; }
        public bool? IsAvailable { get; set; }
    }
}

