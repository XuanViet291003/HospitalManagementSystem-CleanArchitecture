using HospitalManagementSystem.Core.Entities;
using HospitalManagementSystem.Core.Interfaces.Repositories;
using MediatR;

namespace HospitalManagementSystem.Application.Features.DoctorSchedules.Queries.GetDoctorScheduleById
{
    public class GetDoctorScheduleByIdQueryHandler : IRequestHandler<GetDoctorScheduleByIdQuery, DoctorSchedule>
    {
        private readonly IDoctorScheduleRepository _scheduleRepository;

        public GetDoctorScheduleByIdQueryHandler(IDoctorScheduleRepository scheduleRepository)
        {
            _scheduleRepository = scheduleRepository;
        }

        public async Task<DoctorSchedule> Handle(GetDoctorScheduleByIdQuery request, CancellationToken cancellationToken)
        {
            var schedule = await _scheduleRepository.GetByIdAsync(request.Id);
            if (schedule == null)
            {
                throw new InvalidOperationException($"Không tìm thấy lịch làm việc với Id = {request.Id}");
            }
            return schedule;
        }
    }
}

