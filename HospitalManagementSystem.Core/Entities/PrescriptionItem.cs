namespace HospitalManagementSystem.Core.Entities
{
    public class PrescriptionItem : BaseEntity
    {
        public long PrescriptionId { get; set; }
        public Prescription? Prescription { get; set; }
        public long MedicineId { get; set; }
        public Medicine? Medicine { get; set; }
        public required int Quantity { get; set; }
        public required string Dosage { get; set; } // Liều dùng (VD: "Sáng 1 viên, tối 1 viên sau ăn")
        public string? Notes { get; set; }
    }
}


