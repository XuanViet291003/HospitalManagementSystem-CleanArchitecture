using HospitalManagementSystem.Core.Entities;
using HospitalManagementSystem.Core.Interfaces.Repositories;
using MediatR;
using System.Threading;

namespace HospitalManagementSystem.Application.Features.Prescriptions.Commands.CreatePrescription
{
    public class CreatePrescriptionCommandHandler : IRequestHandler<CreatePrescriptionCommand, long>
    {
        private readonly IPrescriptionRepository _prescriptionRepository;
        private readonly IPrescriptionItemRepository _prescriptionItemRepository;
        private readonly IMedicalRecordRepository _medicalRecordRepository;
        private readonly IMedicineRepository _medicineRepository;

        public CreatePrescriptionCommandHandler(
            IPrescriptionRepository prescriptionRepository,
            IPrescriptionItemRepository prescriptionItemRepository,
            IMedicalRecordRepository medicalRecordRepository,
            IMedicineRepository medicineRepository)
        {
            _prescriptionRepository = prescriptionRepository;
            _prescriptionItemRepository = prescriptionItemRepository;
            _medicalRecordRepository = medicalRecordRepository;
            _medicineRepository = medicineRepository;
        }

        public async Task<long> Handle(CreatePrescriptionCommand request, CancellationToken cancellationToken)
        {
            // Validate medical record exists
            var medicalRecord = await _medicalRecordRepository.GetByIdAsync(request.MedicalRecordId);
            if (medicalRecord == null)
            {
                throw new InvalidOperationException($"Không tìm thấy bệnh án với Id = {request.MedicalRecordId}");
            }

            // Check if prescription already exists for this medical record
            var existingPrescription = await _prescriptionRepository.GetByMedicalRecordIdAsync(request.MedicalRecordId);
            if (existingPrescription != null)
            {
                throw new InvalidOperationException($"Đơn thuốc đã tồn tại cho bệnh án này.");
            }

            // Validate all medicines and check stock
            foreach (var item in request.Items)
            {
                var medicine = await _medicineRepository.GetByIdAsync(item.MedicineId);
                if (medicine == null)
                {
                    throw new InvalidOperationException($"Không tìm thấy thuốc với Id = {item.MedicineId}");
                }

                if (!await _medicineRepository.CheckStockAsync(item.MedicineId, item.Quantity))
                {
                    throw new InvalidOperationException($"Thuốc '{medicine.Name}' không đủ tồn kho. Tồn kho hiện tại: {medicine.Stock}, yêu cầu: {item.Quantity}");
                }
            }

            // Create prescription
            var prescription = new Prescription
            {
                MedicalRecordId = request.MedicalRecordId,
                IssuedDate = DateTime.UtcNow,
                CreatedAt = DateTime.UtcNow
            };

            var createdPrescription = await _prescriptionRepository.AddAsync(prescription);

            // Create prescription items and update stock
            foreach (var item in request.Items)
            {
                var medicine = await _medicineRepository.GetByIdAsync(item.MedicineId);
                
                var prescriptionItem = new PrescriptionItem
                {
                    PrescriptionId = createdPrescription.Id,
                    MedicineId = item.MedicineId,
                    Quantity = item.Quantity,
                    Dosage = item.Dosage,
                    Notes = item.Notes,
                    CreatedAt = DateTime.UtcNow
                };

                await _prescriptionItemRepository.AddAsync(prescriptionItem);

                // Update medicine stock
                medicine!.Stock -= item.Quantity;
                medicine.UpdatedAt = DateTime.UtcNow;
                await _medicineRepository.UpdateAsync(medicine);
            }

            return createdPrescription.Id;
        }
    }
}


