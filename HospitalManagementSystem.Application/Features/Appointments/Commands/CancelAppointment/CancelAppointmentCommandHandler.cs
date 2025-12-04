using HospitalManagementSystem.Core.Interfaces.Repositories;
using MediatR;
using System.Threading;

namespace HospitalManagementSystem.Application.Features.Appointments.Commands.CancelAppointment
{
    public class CancelAppointmentCommandHandler : IRequestHandler<CancelAppointmentCommand>
    {
        private readonly IAppointmentRepository _appointmentRepository;

        public CancelAppointmentCommandHandler(IAppointmentRepository appointmentRepository)
        {
            _appointmentRepository = appointmentRepository;
        }

        public async Task Handle(CancelAppointmentCommand request, CancellationToken cancellationToken)
        {
            var appointment = await _appointmentRepository.GetByIdAsync(request.AppointmentId);
            if (appointment == null)
            {
                throw new InvalidOperationException($"Không tìm thấy lịch hẹn với Id = {request.AppointmentId}");
            }

            // Chỉ cho phép hủy nếu chưa completed
            if (appointment.Status == 1) // Completed
            {
                throw new InvalidOperationException("Không thể hủy lịch hẹn đã hoàn thành.");
            }

            if (appointment.Status == 2) // Already cancelled
            {
                throw new InvalidOperationException("Lịch hẹn này đã được hủy trước đó.");
            }

            appointment.Status = 2; // Cancelled
            if (!string.IsNullOrEmpty(request.Reason))
            {
                appointment.Notes = string.IsNullOrEmpty(appointment.Notes) 
                    ? $"Lý do hủy: {request.Reason}" 
                    : $"{appointment.Notes}\nLý do hủy: {request.Reason}";
            }
            appointment.UpdatedAt = DateTime.UtcNow;

            await _appointmentRepository.UpdateAsync(appointment);
        }
    }
}


