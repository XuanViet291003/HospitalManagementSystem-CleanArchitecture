namespace HospitalManagementSystem.Application.DTOs.Prescriptions
{
    public class CreatePrescriptionDto
    {
        public required long MedicalRecordId { get; set; }
        public required List<CreatePrescriptionItemDto> Items { get; set; }
    }

    public class CreatePrescriptionItemDto
    {
        public required long MedicineId { get; set; }
        public required int Quantity { get; set; }
        public required string Dosage { get; set; }
        public string? Notes { get; set; }
    }
}


