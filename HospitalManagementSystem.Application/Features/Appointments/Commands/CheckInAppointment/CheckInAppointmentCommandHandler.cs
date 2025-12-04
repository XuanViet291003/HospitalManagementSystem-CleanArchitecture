using HospitalManagementSystem.Core.Interfaces.Repositories;
using MediatR;
using System.Threading;

namespace HospitalManagementSystem.Application.Features.Appointments.Commands.CheckInAppointment
{
    public class CheckInAppointmentCommandHandler : IRequestHandler<CheckInAppointmentCommand>
    {
        private readonly IAppointmentRepository _appointmentRepository;

        public CheckInAppointmentCommandHandler(IAppointmentRepository appointmentRepository)
        {
            _appointmentRepository = appointmentRepository;
        }

        public async Task Handle(CheckInAppointmentCommand request, CancellationToken cancellationToken)
        {
            var appointment = await _appointmentRepository.GetByIdAsync(request.AppointmentId);
            if (appointment == null)
            {
                throw new InvalidOperationException($"Không tìm thấy lịch hẹn với Id = {request.AppointmentId}");
            }

            if (appointment.Status != 0) // Not Scheduled
            {
                throw new InvalidOperationException("Chỉ có thể check-in lịch hẹn đã được đặt (Scheduled).");
            }

            appointment.Status = 3; // CheckedIn
            appointment.UpdatedAt = DateTime.UtcNow;
            await _appointmentRepository.UpdateAsync(appointment);
        }
    }
}


