using MediatR;

namespace HospitalManagementSystem.Application.Features.MedicalRecords.Commands.CreateMedicalRecord
{
    public class CreateMedicalRecordCommand : IRequest<long>
    {
        public required long AppointmentId { get; set; }
        public string? Symptoms { get; set; }
        public string? Diagnosis { get; set; }
        public string? TreatmentPlan { get; set; }
        public DateTime? FollowUpDate { get; set; }
    }
}


