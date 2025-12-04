using MediatR;

namespace HospitalManagementSystem.Application.Features.Appointments.Commands.CompleteAppointment
{
    public class CompleteAppointmentCommand : IRequest
    {
        public required long AppointmentId { get; set; }
    }
}


