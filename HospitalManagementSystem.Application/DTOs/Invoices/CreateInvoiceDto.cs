namespace HospitalManagementSystem.Application.DTOs.Invoices
{
    public class CreateInvoiceDto
    {
        public required long AppointmentId { get; set; }
        public List<CreateInvoiceItemDto>? Items { get; set; }
        public DateTime? DueDate { get; set; }
    }

    public class CreateInvoiceItemDto
    {
        public required string ItemDescription { get; set; }
        public required string ItemType { get; set; } // "Consultation", "Medicine", "LabTest"
        public required int Quantity { get; set; }
        public required decimal UnitPrice { get; set; }
    }
}


