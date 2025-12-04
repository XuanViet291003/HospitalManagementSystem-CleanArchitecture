using HospitalManagementSystem.Core.Entities;
using HospitalManagementSystem.Core.Interfaces.Repositories;
using HospitalManagementSystem.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagementSystem.Infrastructure.Persistence.Repositories
{
    public class InvoiceItemRepository : IInvoiceItemRepository
    {
        private readonly ApplicationDbContext _context;

        public InvoiceItemRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<InvoiceItem> AddAsync(InvoiceItem invoiceItem)
        {
            await _context.InvoiceItems.AddAsync(invoiceItem);
            await _context.SaveChangesAsync();
            return invoiceItem;
        }

        public async Task<InvoiceItem?> GetByIdAsync(long id)
        {
            return await _context.InvoiceItems
                .Include(ii => ii.Invoice)
                .FirstOrDefaultAsync(ii => ii.Id == id);
        }

        public async Task<IReadOnlyList<InvoiceItem>> GetByInvoiceIdAsync(long invoiceId)
        {
            return await _context.InvoiceItems
                .Where(ii => ii.InvoiceId == invoiceId)
                .OrderBy(ii => ii.CreatedAt)
                .ToListAsync();
        }

        public async Task UpdateAsync(InvoiceItem invoiceItem)
        {
            _context.InvoiceItems.Update(invoiceItem);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(InvoiceItem invoiceItem)
        {
            _context.InvoiceItems.Remove(invoiceItem);
            await _context.SaveChangesAsync();
        }

        public async Task<decimal> CalculateInvoiceTotalAsync(long invoiceId)
        {
            return await _context.InvoiceItems
                .Where(ii => ii.InvoiceId == invoiceId)
                .SumAsync(ii => ii.Amount);
        }
    }
}


