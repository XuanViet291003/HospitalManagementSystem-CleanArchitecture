using AutoMapper;
using HospitalManagementSystem.Application.DTOs.Invoices;
using HospitalManagementSystem.Core.Interfaces.Repositories;
using MediatR;
using System.Threading;

namespace HospitalManagementSystem.Application.Features.Invoices.Queries.GetInvoiceById
{
    public class GetInvoiceByIdQueryHandler : IRequestHandler<GetInvoiceByIdQuery, InvoiceDto>
    {
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly IInvoiceItemRepository _invoiceItemRepository;
        private readonly IMapper _mapper;

        public GetInvoiceByIdQueryHandler(
            IInvoiceRepository invoiceRepository,
            IInvoiceItemRepository invoiceItemRepository,
            IMapper mapper)
        {
            _invoiceRepository = invoiceRepository;
            _invoiceItemRepository = invoiceItemRepository;
            _mapper = mapper;
        }

        public async Task<InvoiceDto> Handle(GetInvoiceByIdQuery request, CancellationToken cancellationToken)
        {
            var invoice = await _invoiceRepository.GetByIdAsync(request.Id);
            if (invoice == null)
            {
                throw new InvalidOperationException($"Không tìm thấy hóa đơn với Id = {request.Id}");
            }

            var items = await _invoiceItemRepository.GetByInvoiceIdAsync(request.Id);
            var invoiceDto = _mapper.Map<InvoiceDto>(invoice);
            invoiceDto.Items = _mapper.Map<List<InvoiceItemDto>>(items);

            return invoiceDto;
        }
    }
}


