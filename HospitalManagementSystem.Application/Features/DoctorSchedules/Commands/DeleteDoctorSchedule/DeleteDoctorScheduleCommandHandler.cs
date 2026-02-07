using HospitalManagementSystem.Core.Interfaces.Repositories;
using MediatR;

namespace HospitalManagementSystem.Application.Features.DoctorSchedules.Commands.DeleteDoctorSchedule
{
    public class DeleteDoctorScheduleCommandHandler : IRequestHandler<DeleteDoctorScheduleCommand>
    {
        private readonly IDoctorScheduleRepository _scheduleRepository;
        private readonly IAppointmentRepository _appointmentRepository;

        public DeleteDoctorScheduleCommandHandler(
            IDoctorScheduleRepository scheduleRepository,
            IAppointmentRepository appointmentRepository)
        {
            _scheduleRepository = scheduleRepository;
            _appointmentRepository = appointmentRepository;
        }

        public async Task Handle(DeleteDoctorScheduleCommand request, CancellationToken cancellationToken)
        {
            var schedule = await _scheduleRepository.GetByIdAsync(request.Id);
            if (schedule == null)
            {
                throw new InvalidOperationException($"Không tìm thấy lịch làm việc với Id = {request.Id}");
            }

            // Check if there are any appointments scheduled for this schedule's time slot
            var appointments = await _appointmentRepository.GetByDoctorIdAsync(schedule.DoctorId, schedule.WorkDate);
            var conflictingAppointments = appointments.Where(a =>
                a.AppointmentTime.Date == schedule.WorkDate.Date &&
                a.AppointmentTime.TimeOfDay >= schedule.StartTime &&
                a.AppointmentTime.TimeOfDay < schedule.EndTime &&
                a.Status != 2); // Not cancelled

            if (conflictingAppointments.Any())
            {
                throw new InvalidOperationException("Không thể xóa lịch làm việc này vì đã có lịch hẹn được đặt trong khoảng thời gian này.");
            }

            await _scheduleRepository.DeleteAsync(schedule);
        }
    }
}

