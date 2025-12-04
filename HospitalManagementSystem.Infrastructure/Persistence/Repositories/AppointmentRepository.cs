using HospitalManagementSystem.Core.Entities;
using HospitalManagementSystem.Core.Interfaces.Repositories;
using HospitalManagementSystem.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagementSystem.Infrastructure.Persistence.Repositories
{
    public class AppointmentRepository : IAppointmentRepository
    {
        private readonly ApplicationDbContext _context;

        public AppointmentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Appointment> AddAsync(Appointment appointment)
        {
            await _context.Appointments.AddAsync(appointment);
            await _context.SaveChangesAsync();
            return appointment;
        }

        public async Task<Appointment?> GetByIdAsync(long id)
        {
            return await _context.Appointments
                .Include(a => a.Patient)
                    .ThenInclude(p => p!.User)
                .Include(a => a.Doctor)
                    .ThenInclude(d => d!.Department)
                .Include(a => a.Doctor)
                    .ThenInclude(d => d!.User)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<IReadOnlyList<Appointment>> GetByPatientIdAsync(long patientId)
        {
            return await _context.Appointments
                .Include(a => a.Doctor)
                    .ThenInclude(d => d!.Department)
                .Include(a => a.Doctor)
                    .ThenInclude(d => d!.User)
                .Where(a => a.PatientId == patientId)
                .OrderByDescending(a => a.AppointmentTime)
                .ToListAsync();
        }

        public async Task<IReadOnlyList<Appointment>> GetByDoctorIdAsync(long doctorId, DateTime? date = null)
        {
            var query = _context.Appointments
                .Include(a => a.Patient)
                    .ThenInclude(p => p!.User)
                .Where(a => a.DoctorId == doctorId);

            if (date.HasValue)
            {
                query = query.Where(a => a.AppointmentTime.Date == date.Value.Date);
            }

            return await query
                .OrderBy(a => a.AppointmentTime)
                .ToListAsync();
        }

        public async Task<IReadOnlyList<Appointment>> GetByDoctorIdAndTimeRangeAsync(long doctorId, DateTime startTime, DateTime endTime)
        {
            return await _context.Appointments
                .Include(a => a.Patient)
                .Where(a => a.DoctorId == doctorId
                    && a.AppointmentTime >= startTime
                    && a.AppointmentTime < endTime
                    && a.Status != 2) // Not cancelled
                .ToListAsync();
        }

        public async Task UpdateAsync(Appointment appointment)
        {
            _context.Appointments.Update(appointment);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> HasConflictingAppointmentAsync(long doctorId, DateTime appointmentTime, int durationMinutes, long? excludeAppointmentId = null)
        {
            var endTime = appointmentTime.AddMinutes(durationMinutes);
            
            var query = _context.Appointments
                .Where(a => a.DoctorId == doctorId
                    && a.Status != 2 // Not cancelled
                    && ((a.AppointmentTime <= appointmentTime && a.AppointmentTime.AddMinutes(a.DurationMinutes) > appointmentTime)
                        || (a.AppointmentTime < endTime && a.AppointmentTime >= appointmentTime)));

            if (excludeAppointmentId.HasValue)
            {
                query = query.Where(a => a.Id != excludeAppointmentId.Value);
            }

            return await query.AnyAsync();
        }
    }
}


