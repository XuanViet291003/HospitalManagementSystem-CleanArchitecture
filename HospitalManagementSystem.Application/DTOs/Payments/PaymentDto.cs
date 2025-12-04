namespace HospitalManagementSystem.Application.DTOs.Payments
{
    public class PaymentDto
    {
        public long Id { get; set; }
        public long InvoiceId { get; set; }
        public required string InvoiceCode { get; set; }
        public required decimal Amount { get; set; }
        public required string PaymentMethod { get; set; }
        public string? TransactionCode { get; set; }
        public required DateTime PaymentDate { get; set; }
    }
}


