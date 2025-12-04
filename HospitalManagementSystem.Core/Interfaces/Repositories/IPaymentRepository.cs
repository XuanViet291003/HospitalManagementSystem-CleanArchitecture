using HospitalManagementSystem.Core.Entities;

namespace HospitalManagementSystem.Core.Interfaces.Repositories
{
    public interface IPaymentRepository
    {
        Task<Payment> AddAsync(Payment payment);
        Task<Payment?> GetByIdAsync(long id);
        Task<IReadOnlyList<Payment>> GetByInvoiceIdAsync(long invoiceId);
        Task<IReadOnlyList<Payment>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<decimal> GetTotalPaidByInvoiceIdAsync(long invoiceId);
        Task UpdateAsync(Payment payment);
    }
}


