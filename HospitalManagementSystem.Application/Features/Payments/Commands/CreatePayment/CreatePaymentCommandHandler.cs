using HospitalManagementSystem.Core.Entities;
using HospitalManagementSystem.Core.Interfaces.Repositories;
using MediatR;
using System.Threading;

namespace HospitalManagementSystem.Application.Features.Payments.Commands.CreatePayment
{
    public class CreatePaymentCommandHandler : IRequestHandler<CreatePaymentCommand, long>
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IInvoiceRepository _invoiceRepository;

        public CreatePaymentCommandHandler(
            IPaymentRepository paymentRepository,
            IInvoiceRepository invoiceRepository)
        {
            _paymentRepository = paymentRepository;
            _invoiceRepository = invoiceRepository;
        }

        public async Task<long> Handle(CreatePaymentCommand request, CancellationToken cancellationToken)
        {
            // Validate invoice exists
            var invoice = await _invoiceRepository.GetByIdAsync(request.InvoiceId);
            if (invoice == null)
            {
                throw new InvalidOperationException($"Không tìm thấy hóa đơn với Id = {request.InvoiceId}");
            }

            // Check if invoice is already paid or cancelled
            if (invoice.Status == 1) // Paid
            {
                throw new InvalidOperationException("Hóa đơn này đã được thanh toán đầy đủ.");
            }

            if (invoice.Status == 2) // Cancelled
            {
                throw new InvalidOperationException("Không thể thanh toán hóa đơn đã bị hủy.");
            }

            // Calculate total paid amount
            var totalPaid = await _paymentRepository.GetTotalPaidByInvoiceIdAsync(request.InvoiceId);
            var remainingAmount = invoice.TotalAmount - totalPaid;

            // Validate payment amount
            if (request.Amount <= 0)
            {
                throw new InvalidOperationException("Số tiền thanh toán phải lớn hơn 0.");
            }

            if (request.Amount > remainingAmount)
            {
                throw new InvalidOperationException($"Số tiền thanh toán ({request.Amount:N0}) vượt quá số tiền còn lại ({remainingAmount:N0}).");
            }

            // Create payment
            var payment = new Payment
            {
                InvoiceId = request.InvoiceId,
                Amount = request.Amount,
                PaymentMethod = request.PaymentMethod,
                TransactionCode = request.TransactionCode,
                PaymentDate = DateTime.UtcNow,
                CreatedAt = DateTime.UtcNow
            };

            var createdPayment = await _paymentRepository.AddAsync(payment);

            // Update invoice status if fully paid
            totalPaid += request.Amount;
            if (totalPaid >= invoice.TotalAmount)
            {
                invoice.Status = 1; // Paid
                invoice.UpdatedAt = DateTime.UtcNow;
                await _invoiceRepository.UpdateAsync(invoice);
            }

            return createdPayment.Id;
        }
    }
}


