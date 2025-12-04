using HospitalManagementSystem.Core.Entities;
using HospitalManagementSystem.Core.Interfaces.Repositories;
using MediatR;
using System.Threading;

namespace HospitalManagementSystem.Application.Features.Invoices.Commands.AddInvoiceItem
{
    public class AddInvoiceItemCommandHandler : IRequestHandler<AddInvoiceItemCommand, long>
    {
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly IInvoiceItemRepository _invoiceItemRepository;

        public AddInvoiceItemCommandHandler(
            IInvoiceRepository invoiceRepository,
            IInvoiceItemRepository invoiceItemRepository)
        {
            _invoiceRepository = invoiceRepository;
            _invoiceItemRepository = invoiceItemRepository;
        }

        public async Task<long> Handle(AddInvoiceItemCommand request, CancellationToken cancellationToken)
        {
            // Validate invoice exists and is not paid/cancelled
            var invoice = await _invoiceRepository.GetByIdAsync(request.InvoiceId);
            if (invoice == null)
            {
                throw new InvalidOperationException($"Không tìm thấy hóa đơn với Id = {request.InvoiceId}");
            }

            if (invoice.Status == 1) // Paid
            {
                throw new InvalidOperationException("Không thể thêm mục vào hóa đơn đã thanh toán.");
            }

            if (invoice.Status == 2) // Cancelled
            {
                throw new InvalidOperationException("Không thể thêm mục vào hóa đơn đã hủy.");
            }

            // Calculate amount
            var amount = request.UnitPrice * request.Quantity;

            // Create invoice item
            var invoiceItem = new InvoiceItem
            {
                InvoiceId = request.InvoiceId,
                ItemDescription = request.ItemDescription,
                ItemType = request.ItemType,
                Quantity = request.Quantity,
                UnitPrice = request.UnitPrice,
                Amount = amount,
                CreatedAt = DateTime.UtcNow
            };

            var createdItem = await _invoiceItemRepository.AddAsync(invoiceItem);

            // Update invoice total amount
            invoice.TotalAmount = await _invoiceItemRepository.CalculateInvoiceTotalAsync(request.InvoiceId);
            invoice.UpdatedAt = DateTime.UtcNow;
            await _invoiceRepository.UpdateAsync(invoice);

            return createdItem.Id;
        }
    }
}


