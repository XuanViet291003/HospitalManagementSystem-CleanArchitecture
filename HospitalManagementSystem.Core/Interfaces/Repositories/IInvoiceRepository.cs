using HospitalManagementSystem.Core.Entities;

namespace HospitalManagementSystem.Core.Interfaces.Repositories
{
    public interface IInvoiceRepository
    {
        Task<Invoice> AddAsync(Invoice invoice);
        Task<Invoice?> GetByIdAsync(long id);
        Task<Invoice?> GetByInvoiceCodeAsync(string invoiceCode);
        Task<Invoice?> GetByAppointmentIdAsync(long appointmentId);
        Task<IReadOnlyList<Invoice>> GetByPatientIdAsync(long patientId);
        Task<IReadOnlyList<Invoice>> GetByStatusAsync(byte status);
        Task UpdateAsync(Invoice invoice);
        Task<string> GenerateInvoiceCodeAsync();
    }
}


