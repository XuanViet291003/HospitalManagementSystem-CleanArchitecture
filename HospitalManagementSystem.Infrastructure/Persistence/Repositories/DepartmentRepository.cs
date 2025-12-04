using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HospitalManagementSystem.Core.Entities;
using HospitalManagementSystem.Core.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagementSystem.Infrastructure.Persistence.Repositories
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly ApplicationDbContext _context;
        public DepartmentRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Department> AddAsync(Department department)
        {
            await _context.Departments.AddAsync(department);
            await _context.SaveChangesAsync();
            return department;
        }

        public async Task<Department?> GetByNameAsync(string name)
        {
            return await _context.Departments   
                .FirstOrDefaultAsync(d => d.Name.ToLower() == name.ToLower());
        }

        public async Task<Department?> GetByIdAsync(long id)
        {
            return await _context.Departments.FindAsync(id);
        }
        public async Task<IReadOnlyList<Department>> GetAllAsync()
        {
            return await _context.Departments
                                 .OrderBy(d => d.Name) 
                                 .ToListAsync();
        }
        public async Task UpdateAsync(Department department)
        {
            _context.Entry(department).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Department department)
        {
            _context.Departments.Remove(department);
            await _context.SaveChangesAsync();
        }
    }
}
