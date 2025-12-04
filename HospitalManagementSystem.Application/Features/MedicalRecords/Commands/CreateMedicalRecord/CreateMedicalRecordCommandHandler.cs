using HospitalManagementSystem.Core.Entities;
using HospitalManagementSystem.Core.Interfaces.Repositories;
using MediatR;
using System.Threading;

namespace HospitalManagementSystem.Application.Features.MedicalRecords.Commands.CreateMedicalRecord
{
    public class CreateMedicalRecordCommandHandler : IRequestHandler<CreateMedicalRecordCommand, long>
    {
        private readonly IMedicalRecordRepository _medicalRecordRepository;
        private readonly IAppointmentRepository _appointmentRepository;

        public CreateMedicalRecordCommandHandler(
            IMedicalRecordRepository medicalRecordRepository,
            IAppointmentRepository appointmentRepository)
        {
            _medicalRecordRepository = medicalRecordRepository;
            _appointmentRepository = appointmentRepository;
        }

        public async Task<long> Handle(CreateMedicalRecordCommand request, CancellationToken cancellationToken)
        {
            // Validate appointment exists
            var appointment = await _appointmentRepository.GetByIdAsync(request.AppointmentId);
            if (appointment == null)
            {
                throw new InvalidOperationException($"Không tìm thấy lịch hẹn với Id = {request.AppointmentId}");
            }

            // Check if medical record already exists for this appointment
            var existingRecord = await _medicalRecordRepository.GetByAppointmentIdAsync(request.AppointmentId);
            if (existingRecord != null)
            {
                throw new InvalidOperationException($"Bệnh án đã tồn tại cho lịch hẹn này.");
            }

            // Only allow creating medical record for completed appointments
            if (appointment.Status != 1) // 1 = Completed
            {
                throw new InvalidOperationException("Chỉ có thể tạo bệnh án cho lịch hẹn đã hoàn thành.");
            }

            var medicalRecord = new MedicalRecord
            {
                AppointmentId = request.AppointmentId,
                PatientId = appointment.PatientId,
                DoctorId = appointment.DoctorId,
                Symptoms = request.Symptoms,
                Diagnosis = request.Diagnosis,
                TreatmentPlan = request.TreatmentPlan,
                FollowUpDate = request.FollowUpDate,
                CreatedAt = DateTime.UtcNow
            };

            var createdRecord = await _medicalRecordRepository.AddAsync(medicalRecord);
            return createdRecord.Id;
        }
    }
}


