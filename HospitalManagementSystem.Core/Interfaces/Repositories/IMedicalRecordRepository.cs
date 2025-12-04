using HospitalManagementSystem.Core.Entities;

namespace HospitalManagementSystem.Core.Interfaces.Repositories
{
    public interface IMedicalRecordRepository
    {
        Task<MedicalRecord> AddAsync(MedicalRecord medicalRecord);
        Task<MedicalRecord?> GetByIdAsync(long id);
        Task<MedicalRecord?> GetByAppointmentIdAsync(long appointmentId);
        Task<IReadOnlyList<MedicalRecord>> GetByPatientIdAsync(long patientId);
        Task<IReadOnlyList<MedicalRecord>> GetByDoctorIdAsync(long doctorId);
        Task UpdateAsync(MedicalRecord medicalRecord);
    }
}


