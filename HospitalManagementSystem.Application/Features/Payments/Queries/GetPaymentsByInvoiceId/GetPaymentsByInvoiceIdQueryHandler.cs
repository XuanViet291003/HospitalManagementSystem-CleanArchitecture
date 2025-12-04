using AutoMapper;
using HospitalManagementSystem.Application.DTOs.Payments;
using HospitalManagementSystem.Core.Interfaces.Repositories;
using MediatR;
using System.Threading;

namespace HospitalManagementSystem.Application.Features.Payments.Queries.GetPaymentsByInvoiceId
{
    public class GetPaymentsByInvoiceIdQueryHandler : IRequestHandler<GetPaymentsByInvoiceIdQuery, List<PaymentDto>>
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IMapper _mapper;

        public GetPaymentsByInvoiceIdQueryHandler(IPaymentRepository paymentRepository, IMapper mapper)
        {
            _paymentRepository = paymentRepository;
            _mapper = mapper;
        }

        public async Task<List<PaymentDto>> Handle(GetPaymentsByInvoiceIdQuery request, CancellationToken cancellationToken)
        {
            var payments = await _paymentRepository.GetByInvoiceIdAsync(request.InvoiceId);
            return _mapper.Map<List<PaymentDto>>(payments);
        }
    }
}


