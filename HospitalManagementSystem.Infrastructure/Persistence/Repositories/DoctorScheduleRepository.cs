using HospitalManagementSystem.Core.Entities;
using HospitalManagementSystem.Core.Interfaces.Repositories;
using HospitalManagementSystem.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagementSystem.Infrastructure.Persistence.Repositories
{
    public class DoctorScheduleRepository : IDoctorScheduleRepository
    {
        private readonly ApplicationDbContext _context;

        public DoctorScheduleRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<DoctorSchedule> AddAsync(DoctorSchedule schedule)
        {
            await _context.DoctorSchedules.AddAsync(schedule);
            await _context.SaveChangesAsync();
            return schedule;
        }

        public async Task<DoctorSchedule?> GetByIdAsync(long id)
        {
            return await _context.DoctorSchedules
                .Include(s => s.Doctor)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<IReadOnlyList<DoctorSchedule>> GetByDoctorIdAndDateRangeAsync(long doctorId, DateTime startDate, DateTime endDate)
        {
            return await _context.DoctorSchedules
                .Include(s => s.Doctor)
                .Where(s => s.DoctorId == doctorId 
                    && s.WorkDate >= startDate.Date 
                    && s.WorkDate <= endDate.Date
                    && s.IsAvailable)
                .OrderBy(s => s.WorkDate)
                .ThenBy(s => s.StartTime)
                .ToListAsync();
        }

        public async Task<IReadOnlyList<DoctorSchedule>> GetAvailableSchedulesByDoctorIdAsync(long doctorId, DateTime date)
        {
            return await _context.DoctorSchedules
                .Include(s => s.Doctor)
                .Where(s => s.DoctorId == doctorId 
                    && s.WorkDate.Date == date.Date
                    && s.IsAvailable)
                .OrderBy(s => s.StartTime)
                .ToListAsync();
        }

        public async Task UpdateAsync(DoctorSchedule schedule)
        {
            _context.DoctorSchedules.Update(schedule);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(DoctorSchedule schedule)
        {
            _context.DoctorSchedules.Remove(schedule);
            await _context.SaveChangesAsync();
        }
    }
}


