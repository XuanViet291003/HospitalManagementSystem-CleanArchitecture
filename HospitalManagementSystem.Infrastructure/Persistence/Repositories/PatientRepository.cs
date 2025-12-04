using HospitalManagementSystem.Core.Entities;
using HospitalManagementSystem.Core.Interfaces.Repositories;
using HospitalManagementSystem.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagementSystem.Infrastructure.Persistence.Repositories
{
    public class PatientRepository : IPatientRepository
    {
        private readonly ApplicationDbContext _context;

        public PatientRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Patient> AddAsync(Patient patient)
        {
            await _context.Patients.AddAsync(patient);
            await _context.SaveChangesAsync();
            return patient;
        }

        public async Task<Patient?> GetByIdAsync(long id)
        {
            return await _context.Patients
                .Include(p => p.User)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Patient?> GetByUserIdAsync(long? userId)
        {
            if (userId == null) return null;
            return await _context.Patients
                .Include(p => p.User)
                .FirstOrDefaultAsync(p => p.UserId == userId);
        }

        public async Task<Patient?> GetByPatientCodeAsync(string patientCode)
        {
            return await _context.Patients
                .Include(p => p.User)
                .FirstOrDefaultAsync(p => p.PatientCode == patientCode);
        }

        public async Task UpdateAsync(Patient patient)
        {
            _context.Patients.Update(patient);
            await _context.SaveChangesAsync();
        }

        public async Task<string> GeneratePatientCodeAsync()
        {
            var today = DateTime.UtcNow;
            var prefix = $"BN{today:yyyyMMdd}";
            
            var lastPatient = await _context.Patients
                .Where(p => p.PatientCode.StartsWith(prefix))
                .OrderByDescending(p => p.PatientCode)
                .FirstOrDefaultAsync();

            int sequence = 1;
            if (lastPatient != null)
            {
                var lastSequence = lastPatient.PatientCode.Substring(prefix.Length);
                if (int.TryParse(lastSequence, out var parsed))
                {
                    sequence = parsed + 1;
                }
            }

            return $"{prefix}{sequence:D4}";
        }
    }
}

