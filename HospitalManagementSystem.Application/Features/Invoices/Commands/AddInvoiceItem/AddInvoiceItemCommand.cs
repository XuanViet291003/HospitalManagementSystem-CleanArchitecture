using MediatR;

namespace HospitalManagementSystem.Application.Features.Invoices.Commands.AddInvoiceItem
{
    public class AddInvoiceItemCommand : IRequest<long>
    {
        public required long InvoiceId { get; set; }
        public required string ItemDescription { get; set; }
        public required string ItemType { get; set; }
        public required int Quantity { get; set; }
        public required decimal UnitPrice { get; set; }
    }
}


