using MediatR;

namespace HospitalManagementSystem.Application.Features.Appointments.Commands.CreateAppointment
{
    public class CreateAppointmentCommand : IRequest<long>
    {
        public required long PatientId { get; set; }
        public required long DoctorId { get; set; }
        public required DateTime AppointmentTime { get; set; }
        public int DurationMinutes { get; set; } = 30;
        public string? Notes { get; set; }
    }
}


