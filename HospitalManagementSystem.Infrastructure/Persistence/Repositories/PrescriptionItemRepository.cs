using HospitalManagementSystem.Core.Entities;
using HospitalManagementSystem.Core.Interfaces.Repositories;
using HospitalManagementSystem.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagementSystem.Infrastructure.Persistence.Repositories
{
    public class PrescriptionItemRepository : IPrescriptionItemRepository
    {
        private readonly ApplicationDbContext _context;

        public PrescriptionItemRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PrescriptionItem> AddAsync(PrescriptionItem prescriptionItem)
        {
            await _context.PrescriptionItems.AddAsync(prescriptionItem);
            await _context.SaveChangesAsync();
            return prescriptionItem;
        }

        public async Task<PrescriptionItem?> GetByIdAsync(long id)
        {
            return await _context.PrescriptionItems
                .Include(pi => pi.Medicine)
                .Include(pi => pi.Prescription)
                .FirstOrDefaultAsync(pi => pi.Id == id);
        }

        public async Task<IReadOnlyList<PrescriptionItem>> GetByPrescriptionIdAsync(long prescriptionId)
        {
            return await _context.PrescriptionItems
                .Include(pi => pi.Medicine)
                .Where(pi => pi.PrescriptionId == prescriptionId)
                .OrderBy(pi => pi.CreatedAt)
                .ToListAsync();
        }

        public async Task UpdateAsync(PrescriptionItem prescriptionItem)
        {
            _context.PrescriptionItems.Update(prescriptionItem);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(PrescriptionItem prescriptionItem)
        {
            _context.PrescriptionItems.Remove(prescriptionItem);
            await _context.SaveChangesAsync();
        }
    }
}


