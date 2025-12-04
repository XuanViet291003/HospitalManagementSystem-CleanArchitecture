namespace HospitalManagementSystem.Core.Entities
{
    public class Payment : BaseEntity
    {
        public long InvoiceId { get; set; }
        public Invoice? Invoice { get; set; }
        public required decimal Amount { get; set; }
        public required string PaymentMethod { get; set; } // "VNPay", "Cash", "Card"
        public string? TransactionCode { get; set; } // Mã giao dịch từ cổng thanh toán
        public required DateTime PaymentDate { get; set; }
    }
}


