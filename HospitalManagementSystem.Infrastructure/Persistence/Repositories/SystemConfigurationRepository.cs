using HospitalManagementSystem.Core.Entities;
using HospitalManagementSystem.Core.Interfaces.Repositories;
using HospitalManagementSystem.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagementSystem.Infrastructure.Persistence.Repositories
{
    public class SystemConfigurationRepository : ISystemConfigurationRepository
    {
        private readonly ApplicationDbContext _context;

        public SystemConfigurationRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<SystemConfiguration?> GetByKeyAsync(string key)
        {
            return await _context.SystemConfigurations
                .FirstOrDefaultAsync(c => c.Key == key);
        }

        public async Task<IReadOnlyList<SystemConfiguration>> GetAllAsync()
        {
            return await _context.SystemConfigurations
                .OrderBy(c => c.Key)
                .ToListAsync();
        }

        public async Task<SystemConfiguration> AddOrUpdateAsync(SystemConfiguration configuration)
        {
            var existing = await _context.SystemConfigurations
                .FirstOrDefaultAsync(c => c.Key == configuration.Key);

            if (existing != null)
            {
                existing.Value = configuration.Value;
                existing.Description = configuration.Description;
                _context.SystemConfigurations.Update(existing);
            }
            else
            {
                await _context.SystemConfigurations.AddAsync(configuration);
            }

            await _context.SaveChangesAsync();
            return existing ?? configuration;
        }

        public async Task DeleteAsync(string key)
        {
            var config = await _context.SystemConfigurations
                .FirstOrDefaultAsync(c => c.Key == key);

            if (config != null)
            {
                _context.SystemConfigurations.Remove(config);
                await _context.SaveChangesAsync();
            }
        }
    }
}


