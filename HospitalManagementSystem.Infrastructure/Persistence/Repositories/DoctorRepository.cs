using HospitalManagementSystem.Core.Entities;
using HospitalManagementSystem.Core.Interfaces.Repositories;
using HospitalManagementSystem.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagementSystem.Infrastructure.Persistence.Repositories
{
    public class DoctorRepository : IDoctorRepository
    {
        private readonly ApplicationDbContext _context;

        public DoctorRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Doctor> AddAsync(Doctor doctor)
        {
            await _context.Doctors.AddAsync(doctor);
            await _context.SaveChangesAsync();
            return doctor;
        }

        public async Task<Doctor?> GetByIdAsync(long id)
        {
            return await _context.Doctors
                .Include(d => d.User)
                .Include(d => d.Department)
                .FirstOrDefaultAsync(d => d.Id == id);
        }

        public async Task<Doctor?> GetByUserIdAsync(long userId)
        {
            return await _context.Doctors
                .Include(d => d.User)
                .Include(d => d.Department)
                .FirstOrDefaultAsync(d => d.UserId == userId);
        }

        public async Task<IReadOnlyList<Doctor>> GetByDepartmentIdAsync(long departmentId)
        {
            return await _context.Doctors
                .Include(d => d.User)
                .Include(d => d.Department)
                .Where(d => d.DepartmentId == departmentId)
                .ToListAsync();
        }

        public async Task<IReadOnlyList<Doctor>> GetAllAsync()
        {
            return await _context.Doctors
                .Include(d => d.User)
                .Include(d => d.Department)
                .ToListAsync();
        }

        public async Task UpdateAsync(Doctor doctor)
        {
            _context.Doctors.Update(doctor);
            await _context.SaveChangesAsync();
        }
    }
}


