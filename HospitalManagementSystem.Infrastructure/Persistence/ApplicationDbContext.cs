using HospitalManagementSystem.Core.Entities; 
using Microsoft.EntityFrameworkCore;

namespace HospitalManagementSystem.Infrastructure.Persistence
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Role> Roles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Role>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(50);
                entity.HasIndex(e => e.Name).IsUnique();
            });
            modelBuilder.Entity<User>(entity =>
            {
                // Đây là các quy tắc cho bảng Users
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(255);
                entity.HasIndex(e => e.Email).IsUnique();
                entity.Property(e => e.PasswordHash).IsRequired();
                entity.HasOne(u => u.Role) // Một User có một Role
                  .WithMany()         // Một Role có thể có nhiều User (không cần định nghĩa navigation property ngược lại)
                  .HasForeignKey(u => u.RoleId) // Liên kết qua khóa ngoại RoleId
                  .IsRequired();     // Bắt buộc mỗi User phải có một Role
            });
            modelBuilder.Entity<Department>(entity =>
            {
                
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(255);
                entity.HasIndex(e => e.Name).IsUnique();
                entity.Property(e => e.Description).HasMaxLength(500);
            });

        }
        }
}