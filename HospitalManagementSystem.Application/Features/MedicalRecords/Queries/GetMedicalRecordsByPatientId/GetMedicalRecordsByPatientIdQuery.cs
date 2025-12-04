using HospitalManagementSystem.Application.DTOs.MedicalRecords;
using MediatR;

namespace HospitalManagementSystem.Application.Features.MedicalRecords.Queries.GetMedicalRecordsByPatientId
{
    public class GetMedicalRecordsByPatientIdQuery : IRequest<List<MedicalRecordDto>>
    {
        public required long PatientId { get; set; }
    }
}


