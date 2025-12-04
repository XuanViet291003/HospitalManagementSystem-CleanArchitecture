using HospitalManagementSystem.Application.DTOs.Medicines;

namespace HospitalManagementSystem.Application.DTOs.Prescriptions
{
    public class PrescriptionDto
    {
        public long Id { get; set; }
        public long MedicalRecordId { get; set; }
        public required DateTime IssuedDate { get; set; }
        public required List<PrescriptionItemDto> Items { get; set; }
        public decimal TotalAmount { get; set; }
    }

    public class PrescriptionItemDto
    {
        public long Id { get; set; }
        public required MedicineDto Medicine { get; set; }
        public required int Quantity { get; set; }
        public required string Dosage { get; set; }
        public string? Notes { get; set; }
        public decimal SubTotal { get; set; }
    }
}

