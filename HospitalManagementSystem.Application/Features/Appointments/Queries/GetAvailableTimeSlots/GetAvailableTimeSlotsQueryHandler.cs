using HospitalManagementSystem.Application.DTOs.Appointments;
using HospitalManagementSystem.Core.Interfaces.Repositories;
using MediatR;
using System.Threading;

namespace HospitalManagementSystem.Application.Features.Appointments.Queries.GetAvailableTimeSlots
{
    public class GetAvailableTimeSlotsQueryHandler : IRequestHandler<GetAvailableTimeSlotsQuery, List<TimeSlotDto>>
    {
        private readonly IDoctorScheduleRepository _scheduleRepository;
        private readonly IAppointmentRepository _appointmentRepository;

        public GetAvailableTimeSlotsQueryHandler(
            IDoctorScheduleRepository scheduleRepository,
            IAppointmentRepository appointmentRepository)
        {
            _scheduleRepository = scheduleRepository;
            _appointmentRepository = appointmentRepository;
        }

        public async Task<List<TimeSlotDto>> Handle(GetAvailableTimeSlotsQuery request, CancellationToken cancellationToken)
        {
            var schedules = await _scheduleRepository.GetAvailableSchedulesByDoctorIdAsync(request.DoctorId, request.Date);
            var existingAppointments = await _appointmentRepository.GetByDoctorIdAsync(request.DoctorId, request.Date);

            var timeSlots = new List<TimeSlotDto>();

            foreach (var schedule in schedules)
            {
                var currentTime = request.Date.Date.Add(schedule.StartTime);
                var endTime = request.Date.Date.Add(schedule.EndTime);

                while (currentTime.AddMinutes(request.SlotDurationMinutes) <= endTime)
                {
                    var slotEndTime = currentTime.AddMinutes(request.SlotDurationMinutes);
                    
                    // Kiểm tra xem slot này có bị conflict với appointment nào không
                    var isConflict = existingAppointments.Any(a => 
                        a.Status != 2 && // Not cancelled
                        ((a.AppointmentTime <= currentTime && a.AppointmentTime.AddMinutes(a.DurationMinutes) > currentTime) ||
                         (a.AppointmentTime < slotEndTime && a.AppointmentTime >= currentTime)));

                    timeSlots.Add(new TimeSlotDto
                    {
                        StartTime = currentTime,
                        EndTime = slotEndTime,
                        IsAvailable = !isConflict
                    });

                    currentTime = slotEndTime;
                }
            }

            return timeSlots;
        }
    }
}


