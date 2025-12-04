using HospitalManagementSystem.Core.Entities;
using HospitalManagementSystem.Core.Interfaces.Repositories;
using HospitalManagementSystem.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagementSystem.Infrastructure.Persistence.Repositories
{
    public class MedicineRepository : IMedicineRepository
    {
        private readonly ApplicationDbContext _context;

        public MedicineRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Medicine> AddAsync(Medicine medicine)
        {
            await _context.Medicines.AddAsync(medicine);
            await _context.SaveChangesAsync();
            return medicine;
        }

        public async Task<Medicine?> GetByIdAsync(long id)
        {
            return await _context.Medicines.FindAsync(id);
        }

        public async Task<Medicine?> GetByNameAsync(string name)
        {
            return await _context.Medicines
                .FirstOrDefaultAsync(m => m.Name.ToLower() == name.ToLower());
        }

        public async Task<IReadOnlyList<Medicine>> GetAllAsync()
        {
            return await _context.Medicines
                .OrderBy(m => m.Name)
                .ToListAsync();
        }

        public async Task<IReadOnlyList<Medicine>> SearchAsync(string? searchTerm)
        {
            var query = _context.Medicines.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(m => m.Name.Contains(searchTerm) || m.Unit.Contains(searchTerm));
            }

            return await query
                .OrderBy(m => m.Name)
                .ToListAsync();
        }

        public async Task UpdateAsync(Medicine medicine)
        {
            _context.Medicines.Update(medicine);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> CheckStockAsync(long medicineId, int quantity)
        {
            var medicine = await _context.Medicines.FindAsync(medicineId);
            return medicine != null && medicine.Stock >= quantity;
        }
    }
}


