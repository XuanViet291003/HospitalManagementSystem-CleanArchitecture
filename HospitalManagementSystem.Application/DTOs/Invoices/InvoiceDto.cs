using HospitalManagementSystem.Application.DTOs.Appointments;

namespace HospitalManagementSystem.Application.DTOs.Invoices
{
    public class InvoiceDto
    {
        public long Id { get; set; }
        public long AppointmentId { get; set; }
        public required string InvoiceCode { get; set; }
        public required decimal TotalAmount { get; set; }
        public required byte Status { get; set; }
        public required DateTime IssuedDate { get; set; }
        public DateTime? DueDate { get; set; }
        public required List<InvoiceItemDto> Items { get; set; }
        public PatientInfoDto? Patient { get; set; }
    }

    public class InvoiceItemDto
    {
        public long Id { get; set; }
        public required string ItemDescription { get; set; }
        public required string ItemType { get; set; }
        public required int Quantity { get; set; }
        public required decimal UnitPrice { get; set; }
        public required decimal Amount { get; set; }
    }
}

