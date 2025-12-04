using MediatR;

namespace HospitalManagementSystem.Application.Features.Payments.Commands.CreatePayment
{
    public class CreatePaymentCommand : IRequest<long>
    {
        public required long InvoiceId { get; set; }
        public required decimal Amount { get; set; }
        public required string PaymentMethod { get; set; }
        public string? TransactionCode { get; set; }
    }
}


