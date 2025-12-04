using AutoMapper;
using HospitalManagementSystem.Application.DTOs.Prescriptions;
using HospitalManagementSystem.Core.Interfaces.Repositories;
using MediatR;
using System.Threading;

namespace HospitalManagementSystem.Application.Features.Prescriptions.Queries.GetPrescriptionById
{
    public class GetPrescriptionByIdQueryHandler : IRequestHandler<GetPrescriptionByIdQuery, PrescriptionDto>
    {
        private readonly IPrescriptionRepository _prescriptionRepository;
        private readonly IPrescriptionItemRepository _prescriptionItemRepository;
        private readonly IMapper _mapper;

        public GetPrescriptionByIdQueryHandler(
            IPrescriptionRepository prescriptionRepository,
            IPrescriptionItemRepository prescriptionItemRepository,
            IMapper mapper)
        {
            _prescriptionRepository = prescriptionRepository;
            _prescriptionItemRepository = prescriptionItemRepository;
            _mapper = mapper;
        }

        public async Task<PrescriptionDto> Handle(GetPrescriptionByIdQuery request, CancellationToken cancellationToken)
        {
            var prescription = await _prescriptionRepository.GetByIdAsync(request.Id);
            if (prescription == null)
            {
                throw new InvalidOperationException($"Không tìm thấy đơn thuốc với Id = {request.Id}");
            }

            var items = await _prescriptionItemRepository.GetByPrescriptionIdAsync(request.Id);
            var prescriptionDto = _mapper.Map<PrescriptionDto>(prescription);
            
            // Map items and calculate totals
            prescriptionDto.Items = _mapper.Map<List<PrescriptionItemDto>>(items);
            prescriptionDto.TotalAmount = prescriptionDto.Items.Sum(item => item.SubTotal);

            return prescriptionDto;
        }
    }
}


