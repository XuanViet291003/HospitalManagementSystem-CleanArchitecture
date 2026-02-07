using HospitalManagementSystem.Core.Entities;
using HospitalManagementSystem.Core.Interfaces.Repositories;
using MediatR;
using AutoMapper;

namespace HospitalManagementSystem.Application.Features.DoctorSchedules.Commands.CreateDoctorSchedule
{
    public class CreateDoctorScheduleCommandHandler : IRequestHandler<CreateDoctorScheduleCommand, long>
    {
        private readonly IDoctorScheduleRepository _scheduleRepository;
        private readonly IDoctorRepository _doctorRepository;

        public CreateDoctorScheduleCommandHandler(
            IDoctorScheduleRepository scheduleRepository,
            IDoctorRepository doctorRepository)
        {
            _scheduleRepository = scheduleRepository;
            _doctorRepository = doctorRepository;
        }

        public async Task<long> Handle(CreateDoctorScheduleCommand request, CancellationToken cancellationToken)
        {
            // Validate doctor exists
            var doctor = await _doctorRepository.GetByIdAsync(request.DoctorId);
            if (doctor == null)
            {
                throw new InvalidOperationException($"Không tìm thấy bác sĩ với Id = {request.DoctorId}");
            }

            if (request.EndTime <= request.StartTime)
            {
                throw new InvalidOperationException("Thời gian kết thúc phải sau thời gian bắt đầu.");
            }

            var scheduleStart = request.WorkDate.Date.Add(request.StartTime);
            if (scheduleStart < DateTime.UtcNow)
            {
                throw new InvalidOperationException("Không thể tạo lịch làm việc trong quá khứ.");
            }


            // Check for conflicting schedules
            var hasConflict = await _scheduleRepository.HasConflictingScheduleAsync(
                request.DoctorId,
                request.WorkDate,
                request.StartTime,
                request.EndTime);

            if (hasConflict)
            {
                throw new InvalidOperationException("Lịch làm việc này bị trùng với lịch làm việc khác của bác sĩ.");
            }

            var schedule = new DoctorSchedule
            {
                DoctorId = request.DoctorId,
                WorkDate = request.WorkDate.Date,
                StartTime = request.StartTime,
                EndTime = request.EndTime,
                SlotDurationMinutes = request.SlotDurationMinutes,
                IsAvailable = request.IsAvailable,
                CreatedAt = DateTime.UtcNow
            };

            var created = await _scheduleRepository.AddAsync(schedule);
            return created.Id;
        }
    }
}

