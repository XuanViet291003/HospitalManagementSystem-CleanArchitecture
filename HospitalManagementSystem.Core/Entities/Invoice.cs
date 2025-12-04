namespace HospitalManagementSystem.Core.Entities
{
    public class Invoice : BaseEntity
    {
        public long AppointmentId { get; set; }
        public Appointment? Appointment { get; set; }
        public long PatientId { get; set; }
        public Patient? Patient { get; set; }
        public required string InvoiceCode { get; set; }
        public required decimal TotalAmount { get; set; }
        public required byte Status { get; set; } // 0:Unpaid, 1:Paid, 2:Cancelled
        public required DateTime IssuedDate { get; set; }
        public DateTime? DueDate { get; set; }
    }
}


