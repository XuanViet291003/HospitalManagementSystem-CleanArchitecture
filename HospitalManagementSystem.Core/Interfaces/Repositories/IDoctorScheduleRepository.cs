using HospitalManagementSystem.Core.Entities;

namespace HospitalManagementSystem.Core.Interfaces.Repositories
{
    public interface IDoctorScheduleRepository
    {
        Task<DoctorSchedule> AddAsync(DoctorSchedule schedule);
        Task<DoctorSchedule?> GetByIdAsync(long id);
        Task<IReadOnlyList<DoctorSchedule>> GetAllAsync();
        Task<IReadOnlyList<DoctorSchedule>> GetByDoctorIdAsync(long doctorId);
        Task<IReadOnlyList<DoctorSchedule>> GetByDoctorIdAndDateRangeAsync(long doctorId, DateTime startDate, DateTime endDate);
        Task<IReadOnlyList<DoctorSchedule>> GetAvailableSchedulesByDoctorIdAsync(long doctorId, DateTime date);
        Task<bool> HasConflictingScheduleAsync(long doctorId, DateTime workDate, TimeSpan startTime, TimeSpan endTime, long? excludeScheduleId = null);
        Task UpdateAsync(DoctorSchedule schedule);
        Task DeleteAsync(DoctorSchedule schedule);
    }
}


