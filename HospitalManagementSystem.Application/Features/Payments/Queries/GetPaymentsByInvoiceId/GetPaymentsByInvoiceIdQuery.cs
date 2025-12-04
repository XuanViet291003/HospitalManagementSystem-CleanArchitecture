using HospitalManagementSystem.Application.DTOs.Payments;
using MediatR;

namespace HospitalManagementSystem.Application.Features.Payments.Queries.GetPaymentsByInvoiceId
{
    public class GetPaymentsByInvoiceIdQuery : IRequest<List<PaymentDto>>
    {
        public required long InvoiceId { get; set; }
    }
}


