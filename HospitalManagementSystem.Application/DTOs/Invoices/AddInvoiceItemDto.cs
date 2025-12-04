namespace HospitalManagementSystem.Application.DTOs.Invoices
{
    public class AddInvoiceItemDto
    {
        public required string ItemDescription { get; set; }
        public required string ItemType { get; set; }
        public required int Quantity { get; set; }
        public required decimal UnitPrice { get; set; }
    }
}


