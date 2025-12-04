using HospitalManagementSystem.Core.Interfaces.Repositories;
using MediatR;
using System.Threading;

namespace HospitalManagementSystem.Application.Features.Appointments.Commands.CompleteAppointment
{
    public class CompleteAppointmentCommandHandler : IRequestHandler<CompleteAppointmentCommand>
    {
        private readonly IAppointmentRepository _appointmentRepository;

        public CompleteAppointmentCommandHandler(IAppointmentRepository appointmentRepository)
        {
            _appointmentRepository = appointmentRepository;
        }

        public async Task Handle(CompleteAppointmentCommand request, CancellationToken cancellationToken)
        {
            var appointment = await _appointmentRepository.GetByIdAsync(request.AppointmentId);
            if (appointment == null)
            {
                throw new InvalidOperationException($"Không tìm thấy lịch hẹn với Id = {request.AppointmentId}");
            }

            if (appointment.Status != 3) // Not CheckedIn
            {
                throw new InvalidOperationException("Chỉ có thể hoàn thành lịch hẹn đã được check-in.");
            }

            appointment.Status = 1; // Completed
            appointment.UpdatedAt = DateTime.UtcNow;
            await _appointmentRepository.UpdateAsync(appointment);
        }
    }
}


