using HospitalManagementSystem.Core.Interfaces.Repositories;
using MediatR;
using System.Threading;

namespace HospitalManagementSystem.Application.Features.MedicalRecords.Commands.UpdateMedicalRecord
{
    public class UpdateMedicalRecordCommandHandler : IRequestHandler<UpdateMedicalRecordCommand>
    {
        private readonly IMedicalRecordRepository _medicalRecordRepository;

        public UpdateMedicalRecordCommandHandler(IMedicalRecordRepository medicalRecordRepository)
        {
            _medicalRecordRepository = medicalRecordRepository;
        }

        public async Task Handle(UpdateMedicalRecordCommand request, CancellationToken cancellationToken)
        {
            var medicalRecord = await _medicalRecordRepository.GetByIdAsync(request.Id);
            if (medicalRecord == null)
            {
                throw new InvalidOperationException($"Không tìm thấy bệnh án với Id = {request.Id}");
            }

            // Update only if value is provided
            if (request.Symptoms != null)
                medicalRecord.Symptoms = request.Symptoms;
            
            if (request.Diagnosis != null)
                medicalRecord.Diagnosis = request.Diagnosis;
            
            if (request.TreatmentPlan != null)
                medicalRecord.TreatmentPlan = request.TreatmentPlan;
            
            if (request.FollowUpDate.HasValue)
                medicalRecord.FollowUpDate = request.FollowUpDate;

            medicalRecord.UpdatedAt = DateTime.UtcNow;

            await _medicalRecordRepository.UpdateAsync(medicalRecord);
        }
    }
}


