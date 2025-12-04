namespace HospitalManagementSystem.Core.Entities
{
    public class Medicine : BaseEntity
    {
        public required string Name { get; set; }
        public required string Unit { get; set; } // Ví dụ: "Viên", "Chai", "Tuýp"
        public required int Stock { get; set; }
        public required decimal UnitPrice { get; set; }
    }
}


