using MediatR;

namespace HospitalManagementSystem.Application.Features.Prescriptions.Commands.CreatePrescription
{
    public class CreatePrescriptionCommand : IRequest<long>
    {
        public required long MedicalRecordId { get; set; }
        public required List<PrescriptionItemRequest> Items { get; set; }
    }

    public class PrescriptionItemRequest
    {
        public required long MedicineId { get; set; }
        public required int Quantity { get; set; }
        public required string Dosage { get; set; }
        public string? Notes { get; set; }
    }
}


