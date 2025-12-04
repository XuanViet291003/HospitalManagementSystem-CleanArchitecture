using HospitalManagementSystem.Core.Entities;

namespace HospitalManagementSystem.Core.Interfaces.Repositories
{
    public interface IInvoiceItemRepository
    {
        Task<InvoiceItem> AddAsync(InvoiceItem invoiceItem);
        Task<InvoiceItem?> GetByIdAsync(long id);
        Task<IReadOnlyList<InvoiceItem>> GetByInvoiceIdAsync(long invoiceId);
        Task UpdateAsync(InvoiceItem invoiceItem);
        Task DeleteAsync(InvoiceItem invoiceItem);
        Task<decimal> CalculateInvoiceTotalAsync(long invoiceId);
    }
}


