using AutoMapper;
using HospitalManagementSystem.Application.DTOs.Medicines;
using HospitalManagementSystem.Core.Interfaces.Repositories;
using MediatR;
using System.Threading;

namespace HospitalManagementSystem.Application.Features.Medicines.Queries.GetMedicines
{
    public class GetMedicinesQueryHandler : IRequestHandler<GetMedicinesQuery, List<MedicineDto>>
    {
        private readonly IMedicineRepository _medicineRepository;
        private readonly IMapper _mapper;

        public GetMedicinesQueryHandler(IMedicineRepository medicineRepository, IMapper mapper)
        {
            _medicineRepository = medicineRepository;
            _mapper = mapper;
        }

        public async Task<List<MedicineDto>> Handle(GetMedicinesQuery request, CancellationToken cancellationToken)
        {
            var medicines = await _medicineRepository.SearchAsync(request.SearchTerm);
            return _mapper.Map<List<MedicineDto>>(medicines);
        }
    }
}


