using HospitalManagementSystem.Core.Entities;
using HospitalManagementSystem.Core.Interfaces.Repositories;
using HospitalManagementSystem.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagementSystem.Infrastructure.Persistence.Repositories
{
    public class MedicalRecordRepository : IMedicalRecordRepository
    {
        private readonly ApplicationDbContext _context;

        public MedicalRecordRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<MedicalRecord> AddAsync(MedicalRecord medicalRecord)
        {
            await _context.MedicalRecords.AddAsync(medicalRecord);
            await _context.SaveChangesAsync();
            return medicalRecord;
        }

        public async Task<MedicalRecord?> GetByIdAsync(long id)
        {
            return await _context.MedicalRecords
                .Include(mr => mr.Appointment)
                .Include(mr => mr.Patient)
                    .ThenInclude(p => p!.User)
                .Include(mr => mr.Doctor)
                    .ThenInclude(d => d!.Department)
                .Include(mr => mr.Doctor)
                    .ThenInclude(d => d!.User)
                .FirstOrDefaultAsync(mr => mr.Id == id);
        }

        public async Task<MedicalRecord?> GetByAppointmentIdAsync(long appointmentId)
        {
            return await _context.MedicalRecords
                .Include(mr => mr.Appointment)
                .Include(mr => mr.Patient)
                .Include(mr => mr.Doctor)
                .FirstOrDefaultAsync(mr => mr.AppointmentId == appointmentId);
        }

        public async Task<IReadOnlyList<MedicalRecord>> GetByPatientIdAsync(long patientId)
        {
            return await _context.MedicalRecords
                .Include(mr => mr.Appointment)
                .Include(mr => mr.Doctor)
                    .ThenInclude(d => d!.Department)
                .Include(mr => mr.Doctor)
                    .ThenInclude(d => d!.User)
                .Where(mr => mr.PatientId == patientId)
                .OrderByDescending(mr => mr.CreatedAt)
                .ToListAsync();
        }

        public async Task<IReadOnlyList<MedicalRecord>> GetByDoctorIdAsync(long doctorId)
        {
            return await _context.MedicalRecords
                .Include(mr => mr.Appointment)
                .Include(mr => mr.Patient)
                    .ThenInclude(p => p!.User)
                .Where(mr => mr.DoctorId == doctorId)
                .OrderByDescending(mr => mr.CreatedAt)
                .ToListAsync();
        }

        public async Task UpdateAsync(MedicalRecord medicalRecord)
        {
            _context.MedicalRecords.Update(medicalRecord);
            await _context.SaveChangesAsync();
        }
    }
}


