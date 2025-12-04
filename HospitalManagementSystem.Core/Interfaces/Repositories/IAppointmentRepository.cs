using HospitalManagementSystem.Core.Entities;

namespace HospitalManagementSystem.Core.Interfaces.Repositories
{
    public interface IAppointmentRepository
    {
        Task<Appointment> AddAsync(Appointment appointment);
        Task<Appointment?> GetByIdAsync(long id);
        Task<IReadOnlyList<Appointment>> GetByPatientIdAsync(long patientId);
        Task<IReadOnlyList<Appointment>> GetByDoctorIdAsync(long doctorId, DateTime? date = null);
        Task<IReadOnlyList<Appointment>> GetByDoctorIdAndTimeRangeAsync(long doctorId, DateTime startTime, DateTime endTime);
        Task UpdateAsync(Appointment appointment);
        Task<bool> HasConflictingAppointmentAsync(long doctorId, DateTime appointmentTime, int durationMinutes, long? excludeAppointmentId = null);
    }
}


