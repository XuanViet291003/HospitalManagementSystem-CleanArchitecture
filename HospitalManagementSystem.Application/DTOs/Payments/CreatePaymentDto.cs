namespace HospitalManagementSystem.Application.DTOs.Payments
{
    public class CreatePaymentDto
    {
        public required long InvoiceId { get; set; }
        public required decimal Amount { get; set; }
        public required string PaymentMethod { get; set; } // "VNPay", "Cash", "Card"
        public string? TransactionCode { get; set; }
    }
}


