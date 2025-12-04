using HospitalManagementSystem.Core.Entities;

namespace HospitalManagementSystem.Core.Interfaces.Repositories
{
    public interface IMedicineRepository
    {
        Task<Medicine> AddAsync(Medicine medicine);
        Task<Medicine?> GetByIdAsync(long id);
        Task<Medicine?> GetByNameAsync(string name);
        Task<IReadOnlyList<Medicine>> GetAllAsync();
        Task<IReadOnlyList<Medicine>> SearchAsync(string? searchTerm);
        Task UpdateAsync(Medicine medicine);
        Task<bool> CheckStockAsync(long medicineId, int quantity);
    }
}


