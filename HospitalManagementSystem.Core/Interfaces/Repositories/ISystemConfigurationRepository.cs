using HospitalManagementSystem.Core.Entities;

namespace HospitalManagementSystem.Core.Interfaces.Repositories
{
    public interface ISystemConfigurationRepository
    {
        Task<SystemConfiguration?> GetByKeyAsync(string key);
        Task<IReadOnlyList<SystemConfiguration>> GetAllAsync();
        Task<SystemConfiguration> AddOrUpdateAsync(SystemConfiguration configuration);
        Task DeleteAsync(string key);
    }
}


