using HospitalManagementSystem.Application.DTOs.MedicalRecords;
using MediatR;

namespace HospitalManagementSystem.Application.Features.MedicalRecords.Queries.GetMedicalRecordById
{
    public class GetMedicalRecordByIdQuery : IRequest<MedicalRecordDto>
    {
        public required long Id { get; set; }
    }
}


