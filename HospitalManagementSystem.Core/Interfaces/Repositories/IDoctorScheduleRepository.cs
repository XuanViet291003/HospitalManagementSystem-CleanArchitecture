using HospitalManagementSystem.Core.Entities;

namespace HospitalManagementSystem.Core.Interfaces.Repositories
{
    public interface IDoctorScheduleRepository
    {
        Task<DoctorSchedule> AddAsync(DoctorSchedule schedule);
        Task<DoctorSchedule?> GetByIdAsync(long id);
        Task<IReadOnlyList<DoctorSchedule>> GetByDoctorIdAndDateRangeAsync(long doctorId, DateTime startDate, DateTime endDate);
        Task<IReadOnlyList<DoctorSchedule>> GetAvailableSchedulesByDoctorIdAsync(long doctorId, DateTime date);
        Task UpdateAsync(DoctorSchedule schedule);
        Task DeleteAsync(DoctorSchedule schedule);
    }
}


