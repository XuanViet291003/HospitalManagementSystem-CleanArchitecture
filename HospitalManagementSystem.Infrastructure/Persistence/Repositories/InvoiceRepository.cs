using HospitalManagementSystem.Core.Entities;
using HospitalManagementSystem.Core.Interfaces.Repositories;
using HospitalManagementSystem.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagementSystem.Infrastructure.Persistence.Repositories
{
    public class InvoiceRepository : IInvoiceRepository
    {
        private readonly ApplicationDbContext _context;

        public InvoiceRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Invoice> AddAsync(Invoice invoice)
        {
            await _context.Invoices.AddAsync(invoice);
            await _context.SaveChangesAsync();
            return invoice;
        }

        public async Task<Invoice?> GetByIdAsync(long id)
        {
            return await _context.Invoices
                .Include(i => i.Appointment)
                .Include(i => i.Patient)
                    .ThenInclude(p => p!.User)
                .FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<Invoice?> GetByInvoiceCodeAsync(string invoiceCode)
        {
            return await _context.Invoices
                .Include(i => i.Appointment)
                .Include(i => i.Patient)
                .FirstOrDefaultAsync(i => i.InvoiceCode == invoiceCode);
        }

        public async Task<Invoice?> GetByAppointmentIdAsync(long appointmentId)
        {
            return await _context.Invoices
                .Include(i => i.Appointment)
                .Include(i => i.Patient)
                .FirstOrDefaultAsync(i => i.AppointmentId == appointmentId);
        }

        public async Task<IReadOnlyList<Invoice>> GetByPatientIdAsync(long patientId)
        {
            return await _context.Invoices
                .Include(i => i.Appointment)
                .Where(i => i.PatientId == patientId)
                .OrderByDescending(i => i.IssuedDate)
                .ToListAsync();
        }

        public async Task<IReadOnlyList<Invoice>> GetByStatusAsync(byte status)
        {
            return await _context.Invoices
                .Include(i => i.Appointment)
                .Include(i => i.Patient)
                .Where(i => i.Status == status)
                .OrderByDescending(i => i.IssuedDate)
                .ToListAsync();
        }

        public async Task UpdateAsync(Invoice invoice)
        {
            _context.Invoices.Update(invoice);
            await _context.SaveChangesAsync();
        }

        public async Task<string> GenerateInvoiceCodeAsync()
        {
            var today = DateTime.UtcNow;
            var prefix = $"INV{today:yyyyMMdd}";
            
            var lastInvoice = await _context.Invoices
                .Where(i => i.InvoiceCode.StartsWith(prefix))
                .OrderByDescending(i => i.InvoiceCode)
                .FirstOrDefaultAsync();

            int sequence = 1;
            if (lastInvoice != null)
            {
                var lastSequence = lastInvoice.InvoiceCode.Substring(prefix.Length);
                if (int.TryParse(lastSequence, out var parsed))
                {
                    sequence = parsed + 1;
                }
            }

            return $"{prefix}{sequence:D4}";
        }
    }
}


