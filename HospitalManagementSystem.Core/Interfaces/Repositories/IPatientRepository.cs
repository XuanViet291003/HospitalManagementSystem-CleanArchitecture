using HospitalManagementSystem.Core.Entities;

namespace HospitalManagementSystem.Core.Interfaces.Repositories
{
    public interface IPatientRepository
    {
        Task<Patient> AddAsync(Patient patient);
        Task<Patient?> GetByIdAsync(long id);
        Task<Patient?> GetByUserIdAsync(long? userId);
        Task<Patient?> GetByPatientCodeAsync(string patientCode);
        Task UpdateAsync(Patient patient);
        Task<string> GeneratePatientCodeAsync();
    }
}

