using AutoMapper;
using HospitalManagementSystem.Application.DTOs.Invoices;
using HospitalManagementSystem.Core.Interfaces.Repositories;
using MediatR;
using System.Threading;

namespace HospitalManagementSystem.Application.Features.Invoices.Queries.GetInvoicesByPatientId
{
    public class GetInvoicesByPatientIdQueryHandler : IRequestHandler<GetInvoicesByPatientIdQuery, List<InvoiceDto>>
    {
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly IInvoiceItemRepository _invoiceItemRepository;
        private readonly IMapper _mapper;

        public GetInvoicesByPatientIdQueryHandler(
            IInvoiceRepository invoiceRepository,
            IInvoiceItemRepository invoiceItemRepository,
            IMapper mapper)
        {
            _invoiceRepository = invoiceRepository;
            _invoiceItemRepository = invoiceItemRepository;
            _mapper = mapper;
        }

        public async Task<List<InvoiceDto>> Handle(GetInvoicesByPatientIdQuery request, CancellationToken cancellationToken)
        {
            var invoices = await _invoiceRepository.GetByPatientIdAsync(request.PatientId);
            var invoiceDtos = new List<InvoiceDto>();

            foreach (var invoice in invoices)
            {
                var items = await _invoiceItemRepository.GetByInvoiceIdAsync(invoice.Id);
                var invoiceDto = _mapper.Map<InvoiceDto>(invoice);
                invoiceDto.Items = _mapper.Map<List<InvoiceItemDto>>(items);
                invoiceDtos.Add(invoiceDto);
            }

            return invoiceDtos;
        }
    }
}


