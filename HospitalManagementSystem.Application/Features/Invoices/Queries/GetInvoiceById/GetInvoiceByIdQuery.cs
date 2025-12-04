using HospitalManagementSystem.Application.DTOs.Invoices;
using MediatR;

namespace HospitalManagementSystem.Application.Features.Invoices.Queries.GetInvoiceById
{
    public class GetInvoiceByIdQuery : IRequest<InvoiceDto>
    {
        public required long Id { get; set; }
    }
}


