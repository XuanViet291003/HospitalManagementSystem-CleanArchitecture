using HospitalManagementSystem.Core.Entities;

namespace HospitalManagementSystem.Core.Interfaces.Repositories
{
    public interface IPrescriptionItemRepository
    {
        Task<PrescriptionItem> AddAsync(PrescriptionItem prescriptionItem);
        Task<PrescriptionItem?> GetByIdAsync(long id);
        Task<IReadOnlyList<PrescriptionItem>> GetByPrescriptionIdAsync(long prescriptionId);
        Task UpdateAsync(PrescriptionItem prescriptionItem);
        Task DeleteAsync(PrescriptionItem prescriptionItem);
    }
}


