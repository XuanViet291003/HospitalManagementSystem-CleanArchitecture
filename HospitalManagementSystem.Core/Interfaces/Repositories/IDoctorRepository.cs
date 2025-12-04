using HospitalManagementSystem.Core.Entities;

namespace HospitalManagementSystem.Core.Interfaces.Repositories
{
    public interface IDoctorRepository
    {
        Task<Doctor> AddAsync(Doctor doctor);
        Task<Doctor?> GetByIdAsync(long id);
        Task<Doctor?> GetByUserIdAsync(long userId);
        Task<IReadOnlyList<Doctor>> GetByDepartmentIdAsync(long departmentId);
        Task<IReadOnlyList<Doctor>> GetAllAsync();
        Task UpdateAsync(Doctor doctor);
    }
}


