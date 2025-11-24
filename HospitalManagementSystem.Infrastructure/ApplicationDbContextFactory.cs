using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;

namespace HospitalManagementSystem.Infrastructure.Persistence
{
    // Class này chỉ được sử dụng bởi các công cụ dòng lệnh của EF Core (như khi tạo migration)
    // Nó sẽ không được gọi khi ứng dụng chạy bình thường.
    public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            // Bước 1: Tạo một đối tượng ConfigurationBuilder để đọc file cấu hình
            IConfigurationRoot configuration = new ConfigurationBuilder()
                // Đặt đường dẫn cơ sở là thư mục của project API, nơi chứa file appsettings.json
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../HospitalManagementSystem.API"))
                // Thêm file appsettings.json vào để đọc
                .AddJsonFile("appsettings.json")
                // Thêm User Secrets vào (rất quan trọng để lấy connection string bí mật)
                .AddUserSecrets<ApplicationDbContextFactory>() // Dùng chính class này để tìm User Secrets ID
                .Build();

            // Bước 2: Tạo một DbContextOptionsBuilder
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();

            // Bước 3: Lấy chuỗi kết nối từ configuration
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("The connection string 'DefaultConnection' was not found in configuration. Please ensure it is set in appsettings.json or user secrets.");
            }

            // Bước 4: Cấu hình cho builder để sử dụng SQL Server (hoặc MySQL tùy mày)
            optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)); 

            // Bước 5: Tạo và trả về một instance của ApplicationDbContext
            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }
}