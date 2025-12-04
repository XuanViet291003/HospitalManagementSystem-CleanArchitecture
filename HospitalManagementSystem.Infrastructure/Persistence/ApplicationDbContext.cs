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
        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<DoctorSchedule> DoctorSchedules { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<MedicalRecord> MedicalRecords { get; set; }
        public DbSet<Medicine> Medicines { get; set; }
        public DbSet<Prescription> Prescriptions { get; set; }
        public DbSet<PrescriptionItem> PrescriptionItems { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<InvoiceItem> InvoiceItems { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<SystemConfiguration> SystemConfigurations { get; set; }

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

            modelBuilder.Entity<UserProfile>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.FullName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.PhoneNumber).HasMaxLength(20);
                entity.Property(e => e.Address).HasMaxLength(500);
                entity.Property(e => e.AvatarUrl).HasMaxLength(500);
                entity.HasOne(up => up.User)
                    .WithOne()
                    .HasForeignKey<UserProfile>(up => up.UserId)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Cascade);
                entity.HasIndex(e => e.UserId).IsUnique();
            });

            modelBuilder.Entity<Doctor>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Specialization).IsRequired().HasMaxLength(255);
                entity.Property(e => e.LicenseNumber).IsRequired().HasMaxLength(50);
                entity.HasIndex(e => e.LicenseNumber).IsUnique();
                entity.Property(e => e.ConsultationFee).HasColumnType("decimal(18,2)");
                entity.HasOne(d => d.User)
                    .WithOne()
                    .HasForeignKey<Doctor>(d => d.UserId)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(d => d.Department)
                    .WithMany()
                    .HasForeignKey(d => d.DepartmentId)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Restrict);
                entity.HasIndex(e => e.UserId).IsUnique();
            });

            modelBuilder.Entity<Patient>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.PatientCode).IsRequired().HasMaxLength(20);
                entity.HasIndex(e => e.PatientCode).IsUnique();
                entity.Property(e => e.InsuranceNumber).HasMaxLength(50);
                entity.HasOne(p => p.User)
                    .WithOne()
                    .HasForeignKey<Patient>(p => p.UserId)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            modelBuilder.Entity<DoctorSchedule>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.WorkDate).IsRequired();
                entity.HasOne(ds => ds.Doctor)
                    .WithMany()
                    .HasForeignKey(ds => ds.DoctorId)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Cascade);
                entity.HasIndex(e => new { e.DoctorId, e.WorkDate, e.StartTime });
            });

            modelBuilder.Entity<Appointment>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.AppointmentTime).IsRequired();
                entity.HasOne(a => a.Patient)
                    .WithMany()
                    .HasForeignKey(a => a.PatientId)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(a => a.Doctor)
                    .WithMany()
                    .HasForeignKey(a => a.DoctorId)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Restrict);
                entity.HasIndex(e => new { e.DoctorId, e.AppointmentTime });
            });

            modelBuilder.Entity<MedicalRecord>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasOne(mr => mr.Appointment)
                    .WithOne()
                    .HasForeignKey<MedicalRecord>(mr => mr.AppointmentId)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(mr => mr.Patient)
                    .WithMany()
                    .HasForeignKey(mr => mr.PatientId)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(mr => mr.Doctor)
                    .WithMany()
                    .HasForeignKey(mr => mr.DoctorId)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Restrict);
                entity.HasIndex(e => e.AppointmentId).IsUnique();
                entity.HasIndex(e => e.PatientId);
                entity.HasIndex(e => e.DoctorId);
            });

            modelBuilder.Entity<Medicine>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(255);
                entity.Property(e => e.Unit).IsRequired().HasMaxLength(50);
                entity.Property(e => e.UnitPrice).HasColumnType("decimal(18,2)");
                entity.HasIndex(e => e.Name);
            });

            modelBuilder.Entity<Prescription>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.IssuedDate).IsRequired();
                entity.HasOne(p => p.MedicalRecord)
                    .WithMany()
                    .HasForeignKey(p => p.MedicalRecordId)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Restrict);
                entity.HasIndex(e => e.MedicalRecordId);
            });

            modelBuilder.Entity<PrescriptionItem>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Dosage).IsRequired().HasMaxLength(255);
                entity.Property(e => e.Notes).HasMaxLength(500);
                entity.HasOne(pi => pi.Prescription)
                    .WithMany()
                    .HasForeignKey(pi => pi.PrescriptionId)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(pi => pi.Medicine)
                    .WithMany()
                    .HasForeignKey(pi => pi.MedicineId)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Restrict);
                entity.HasIndex(e => e.PrescriptionId);
            });

            modelBuilder.Entity<Invoice>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.InvoiceCode).IsRequired().HasMaxLength(30);
                entity.HasIndex(e => e.InvoiceCode).IsUnique();
                entity.Property(e => e.TotalAmount).HasColumnType("decimal(18,2)");
                entity.HasOne(i => i.Appointment)
                    .WithMany()
                    .HasForeignKey(i => i.AppointmentId)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(i => i.Patient)
                    .WithMany()
                    .HasForeignKey(i => i.PatientId)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Restrict);
                entity.HasIndex(e => e.AppointmentId);
                entity.HasIndex(e => e.PatientId);
            });

            modelBuilder.Entity<InvoiceItem>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.ItemDescription).IsRequired().HasMaxLength(255);
                entity.Property(e => e.ItemType).IsRequired().HasMaxLength(50);
                entity.Property(e => e.UnitPrice).HasColumnType("decimal(18,2)");
                entity.Property(e => e.Amount).HasColumnType("decimal(18,2)");
                entity.HasOne(ii => ii.Invoice)
                    .WithMany()
                    .HasForeignKey(ii => ii.InvoiceId)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Cascade);
                entity.HasIndex(e => e.InvoiceId);
            });

            modelBuilder.Entity<Payment>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Amount).HasColumnType("decimal(18,2)");
                entity.Property(e => e.PaymentMethod).IsRequired().HasMaxLength(50);
                entity.Property(e => e.TransactionCode).HasMaxLength(100);
                entity.HasOne(p => p.Invoice)
                    .WithMany()
                    .HasForeignKey(p => p.InvoiceId)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Restrict);
                entity.HasIndex(e => e.InvoiceId);
                entity.HasIndex(e => e.PaymentDate);
            });

            modelBuilder.Entity<SystemConfiguration>(entity =>
            {
                entity.HasKey(e => e.Key);
                entity.Property(e => e.Key).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Value).IsRequired();
                entity.Property(e => e.Description).HasMaxLength(500);
            });

        }
    }
}