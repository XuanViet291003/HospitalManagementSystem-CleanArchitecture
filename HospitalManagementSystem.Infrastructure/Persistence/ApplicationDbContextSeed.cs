using HospitalManagementSystem.Core.Entities;
using System.Linq;
using System.Threading.Tasks;

namespace HospitalManagementSystem.Infrastructure.Persistence
{
    public static class ApplicationDbContextSeed
    {
        public static async Task SeedDefaultRolesAsync(ApplicationDbContext context)
        {
            if (!context.Roles.Any())
            {
                var roles = new[]
                {
                        new Role { Name = "Admin", CreatedAt = DateTime.UtcNow },
                        new Role { Name = "Doctor", CreatedAt = DateTime.UtcNow },
                        new Role { Name = "Patient", CreatedAt = DateTime.UtcNow }
                    };
                await context.Roles.AddRangeAsync(roles);
                await context.SaveChangesAsync();
            }
        }
    }
}