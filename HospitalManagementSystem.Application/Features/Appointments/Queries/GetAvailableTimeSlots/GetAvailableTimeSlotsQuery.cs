using HospitalManagementSystem.Application.DTOs.Appointments;
using MediatR;

namespace HospitalManagementSystem.Application.Features.Appointments.Queries.GetAvailableTimeSlots
{
    public class GetAvailableTimeSlotsQuery : IRequest<List<TimeSlotDto>>
    {
        public required long DoctorId { get; set; }
        public required DateTime Date { get; set; }
        public int SlotDurationMinutes { get; set; } = 30;
    }
}


