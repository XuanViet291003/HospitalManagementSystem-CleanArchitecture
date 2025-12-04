using MediatR;

namespace HospitalManagementSystem.Application.Features.Appointments.Commands.CheckInAppointment
{
    public class CheckInAppointmentCommand : IRequest
    {
        public required long AppointmentId { get; set; }
    }
}


