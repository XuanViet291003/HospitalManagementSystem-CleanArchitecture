using MediatR;

namespace HospitalManagementSystem.Application.Features.Invoices.Commands.CreateInvoice
{
    public class CreateInvoiceCommand : IRequest<long>
    {
        public required long AppointmentId { get; set; }
        public List<InvoiceItemRequest>? Items { get; set; }
        public DateTime? DueDate { get; set; }
    }

    public class InvoiceItemRequest
    {
        public required string ItemDescription { get; set; }
        public required string ItemType { get; set; }
        public required int Quantity { get; set; }
        public required decimal UnitPrice { get; set; }
    }
}


