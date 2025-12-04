using MediatR;

namespace HospitalManagementSystem.Application.Features.MedicalRecords.Commands.UpdateMedicalRecord
{
    public class UpdateMedicalRecordCommand : IRequest
    {
        public required long Id { get; set; }
        public string? Symptoms { get; set; }
        public string? Diagnosis { get; set; }
        public string? TreatmentPlan { get; set; }
        public DateTime? FollowUpDate { get; set; }
    }
}


