using HospitalManagementSystem.Core.Entities;
using HospitalManagementSystem.Core.Interfaces.Repositories;
using HospitalManagementSystem.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagementSystem.Infrastructure.Persistence.Repositories
{
    public class PrescriptionRepository : IPrescriptionRepository
    {
        private readonly ApplicationDbContext _context;

        public PrescriptionRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Prescription> AddAsync(Prescription prescription)
        {
            await _context.Prescriptions.AddAsync(prescription);
            await _context.SaveChangesAsync();
            return prescription;
        }

        public async Task<Prescription?> GetByIdAsync(long id)
        {
            return await _context.Prescriptions
                .Include(p => p.MedicalRecord)
                    .ThenInclude(mr => mr!.Appointment)
                .Include(p => p.MedicalRecord)
                    .ThenInclude(mr => mr!.Patient)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Prescription?> GetByMedicalRecordIdAsync(long medicalRecordId)
        {
            return await _context.Prescriptions
                .Include(p => p.MedicalRecord)
                .FirstOrDefaultAsync(p => p.MedicalRecordId == medicalRecordId);
        }

        public async Task<IReadOnlyList<Prescription>> GetByPatientIdAsync(long patientId)
        {
            return await _context.Prescriptions
                .Include(p => p.MedicalRecord)
                    .ThenInclude(mr => mr!.Doctor)
                .Where(p => p.MedicalRecord != null && p.MedicalRecord.PatientId == patientId)
                .OrderByDescending(p => p.IssuedDate)
                .ToListAsync();
        }

        public async Task UpdateAsync(Prescription prescription)
        {
            _context.Prescriptions.Update(prescription);
            await _context.SaveChangesAsync();
        }
    }
}


