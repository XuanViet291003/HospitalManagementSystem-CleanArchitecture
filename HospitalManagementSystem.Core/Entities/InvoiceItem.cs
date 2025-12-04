namespace HospitalManagementSystem.Core.Entities
{
    public class InvoiceItem : BaseEntity
    {
        public long InvoiceId { get; set; }
        public Invoice? Invoice { get; set; }
        public required string ItemDescription { get; set; }
        public required string ItemType { get; set; } // "Consultation", "Medicine", "LabTest"
        public required int Quantity { get; set; }
        public required decimal UnitPrice { get; set; }
        public required decimal Amount { get; set; }
    }
}


