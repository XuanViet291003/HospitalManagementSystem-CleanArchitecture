using HospitalManagementSystem.Core.Entities;

namespace HospitalManagementSystem.Core.Interfaces.Repositories
{
    public interface IDepartmentRepository
    {
        Task<Department> AddAsync(Department department);
        Task<Department?> GetByNameAsync(string name);
        Task<Department?> GetByIdAsync(long DepartmentId);
        Task<IReadOnlyList<Department>> GetAllAsync();
        Task UpdateAsync(Department department);
        Task DeleteAsync(Department department);
    }
}
