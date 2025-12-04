namespace HospitalManagementSystem.Application.DTOs.Medicines
{
    public class MedicineDto
    {
        public long Id { get; set; }
        public required string Name { get; set; }
        public required string Unit { get; set; }
        public required int Stock { get; set; }
        public required decimal UnitPrice { get; set; }
    }
}


