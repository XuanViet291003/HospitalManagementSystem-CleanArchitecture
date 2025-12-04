using HospitalManagementSystem.Core.Entities;

namespace HospitalManagementSystem.Core.Interfaces.Repositories
{
    public interface IPrescriptionRepository
    {
        Task<Prescription> AddAsync(Prescription prescription);
        Task<Prescription?> GetByIdAsync(long id);
        Task<Prescription?> GetByMedicalRecordIdAsync(long medicalRecordId);
        Task<IReadOnlyList<Prescription>> GetByPatientIdAsync(long patientId);
        Task UpdateAsync(Prescription prescription);
    }
}


