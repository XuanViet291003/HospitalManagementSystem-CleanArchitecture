using HospitalManagementSystem.Application.DTOs.Invoices;
using MediatR;

namespace HospitalManagementSystem.Application.Features.Invoices.Queries.GetInvoicesByPatientId
{
    public class GetInvoicesByPatientIdQuery : IRequest<List<InvoiceDto>>
    {
        public required long PatientId { get; set; }
    }
}


