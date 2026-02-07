using HospitalManagementSystem.Core.Interfaces.Repositories;
using MediatR;
using HospitalManagementSystem.Core.Entities;
using AutoMapper;

namespace HospitalManagementSystem.Application.Features.DoctorSchedules.Commands.UpdateDoctorSchedule
{
    public class UpdateDoctorScheduleCommandHandler : IRequestHandler<UpdateDoctorScheduleCommand>
    {
        private readonly IDoctorScheduleRepository _scheduleRepository;

        public UpdateDoctorScheduleCommandHandler(IDoctorScheduleRepository scheduleRepository)
        {
            _scheduleRepository = scheduleRepository;
        }

        public async Task Handle(UpdateDoctorScheduleCommand request, CancellationToken cancellationToken)
        {
            var schedule = await _scheduleRepository.GetByIdAsync(request.Id);
            if (schedule == null)
            {
                throw new InvalidOperationException($"Không tìm thấy lịch làm việc với Id = {request.Id}");
            }

            // Validate time range if updating times
            if (request.StartTime.HasValue && request.EndTime.HasValue)
            {
                if (request.EndTime.Value <= request.StartTime.Value)
                {
                    throw new InvalidOperationException("Thời gian kết thúc phải sau thời gian bắt đầu.");
                }
            }
            else if (request.StartTime.HasValue && request.EndTime == null)
            {
                if (request.StartTime.Value >= schedule.EndTime)
                {
                    throw new InvalidOperationException("Thời gian bắt đầu phải trước thời gian kết thúc.");
                }
            }
            else if (request.EndTime.HasValue && request.StartTime == null)
            {
                if (request.EndTime.Value <= schedule.StartTime)
                {
                    throw new InvalidOperationException("Thời gian kết thúc phải sau thời gian bắt đầu.");
                }
            }

            // Check for conflicting schedules if updating date or times
            if (request.WorkDate.HasValue || request.StartTime.HasValue || request.EndTime.HasValue)
            {
                var workDate = request.WorkDate ?? schedule.WorkDate;
                var startTime = request.StartTime ?? schedule.StartTime;
                var endTime = request.EndTime ?? schedule.EndTime;

                var scheduleStart = workDate.Date.Add(startTime);
                if (scheduleStart < DateTime.UtcNow)
                {
                    throw new InvalidOperationException("Không thể cập nhật lịch làm việc sang thời điểm trong quá khứ.");
                }

                var hasConflict = await _scheduleRepository.HasConflictingScheduleAsync(
                    schedule.DoctorId,
                    workDate,
                    startTime,
                    endTime,
                    request.Id); // Exclude current schedule

                if (hasConflict)
                {
                    throw new InvalidOperationException("Lịch làm việc này bị trùng với lịch làm việc khác của bác sĩ.");
                }
            }

            // Update only if value is provided
            if (request.WorkDate.HasValue)
                schedule.WorkDate = request.WorkDate.Value.Date;

            if (request.StartTime.HasValue)
                schedule.StartTime = request.StartTime.Value;

            if (request.EndTime.HasValue)
                schedule.EndTime = request.EndTime.Value;

            if (request.SlotDurationMinutes.HasValue)
                schedule.SlotDurationMinutes = request.SlotDurationMinutes.Value;

            if (request.IsAvailable.HasValue)
                schedule.IsAvailable = request.IsAvailable.Value;

            schedule.UpdatedAt = DateTime.UtcNow;

            await _scheduleRepository.UpdateAsync(schedule);
        }
    }
}

