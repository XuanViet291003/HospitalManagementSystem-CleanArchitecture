using HospitalManagementSystem.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
