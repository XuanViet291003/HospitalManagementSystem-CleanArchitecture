using MediatR;

namespace HospitalManagementSystem.Application.Features.Appointments.Commands.CancelAppointment
{
    public class CancelAppointmentCommand : IRequest
    {
        public required long AppointmentId { get; set; }
        public string? Reason { get; set; }
    }
}


