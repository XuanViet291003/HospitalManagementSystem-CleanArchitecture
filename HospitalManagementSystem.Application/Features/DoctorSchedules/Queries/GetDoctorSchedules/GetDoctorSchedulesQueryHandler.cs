using HospitalManagementSystem.Core.Entities;
using HospitalManagementSystem.Core.Interfaces.Repositories;
using MediatR;

namespace HospitalManagementSystem.Application.Features.DoctorSchedules.Queries.GetDoctorSchedules
{
    public class GetDoctorSchedulesQueryHandler : IRequestHandler<GetDoctorSchedulesQuery, List<DoctorSchedule>>
    {
        private readonly IDoctorScheduleRepository _scheduleRepository;

        public GetDoctorSchedulesQueryHandler(IDoctorScheduleRepository scheduleRepository)
        {
            _scheduleRepository = scheduleRepository;
        }

        public async Task<List<DoctorSchedule>> Handle(GetDoctorSchedulesQuery request, CancellationToken cancellationToken)
        {
            var start = request.StartDate ?? DateTime.UtcNow.Date;
            var end = request.EndDate ?? start.AddDays(30);
            var schedules = await _scheduleRepository.GetByDoctorIdAndDateRangeAsync(request.DoctorId, start, end);
            return schedules.ToList();
        }
    }
}

