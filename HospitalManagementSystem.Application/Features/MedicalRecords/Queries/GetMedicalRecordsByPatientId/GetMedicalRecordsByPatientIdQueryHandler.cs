using AutoMapper;
using HospitalManagementSystem.Application.DTOs.MedicalRecords;
using HospitalManagementSystem.Core.Interfaces.Repositories;
using MediatR;
using System.Threading;

namespace HospitalManagementSystem.Application.Features.MedicalRecords.Queries.GetMedicalRecordsByPatientId
{
    public class GetMedicalRecordsByPatientIdQueryHandler : IRequestHandler<GetMedicalRecordsByPatientIdQuery, List<MedicalRecordDto>>
    {
        private readonly IMedicalRecordRepository _medicalRecordRepository;
        private readonly IMapper _mapper;

        public GetMedicalRecordsByPatientIdQueryHandler(IMedicalRecordRepository medicalRecordRepository, IMapper mapper)
        {
            _medicalRecordRepository = medicalRecordRepository;
            _mapper = mapper;
        }

        public async Task<List<MedicalRecordDto>> Handle(GetMedicalRecordsByPatientIdQuery request, CancellationToken cancellationToken)
        {
            var medicalRecords = await _medicalRecordRepository.GetByPatientIdAsync(request.PatientId);
            return _mapper.Map<List<MedicalRecordDto>>(medicalRecords);
        }
    }
}


