using HospitalManagementSystem.Core.Entities;
using HospitalManagementSystem.Core.Interfaces.Repositories;
using MediatR;
using System.Threading;

namespace HospitalManagementSystem.Application.Features.Appointments.Commands.CreateAppointment
{
    public class CreateAppointmentCommandHandler : IRequestHandler<CreateAppointmentCommand, long>
    {
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IPatientRepository _patientRepository;
        private readonly IDoctorRepository _doctorRepository;

        public CreateAppointmentCommandHandler(
            IAppointmentRepository appointmentRepository,
            IPatientRepository patientRepository,
            IDoctorRepository doctorRepository)
        {
            _appointmentRepository = appointmentRepository;
            _patientRepository = patientRepository;
            _doctorRepository = doctorRepository;
        }

        public async Task<long> Handle(CreateAppointmentCommand request, CancellationToken cancellationToken)
        {
            // Validate patient exists
            var patient = await _patientRepository.GetByIdAsync(request.PatientId);
            if (patient == null)
            {
                throw new InvalidOperationException($"Không tìm thấy bệnh nhân với Id = {request.PatientId}");
            }

            // Validate doctor exists
            var doctor = await _doctorRepository.GetByIdAsync(request.DoctorId);
            if (doctor == null)
            {
                throw new InvalidOperationException($"Không tìm thấy bác sĩ với Id = {request.DoctorId}");
            }

            // Check for conflicts
            var hasConflict = await _appointmentRepository.HasConflictingAppointmentAsync(
                request.DoctorId, 
                request.AppointmentTime, 
                request.DurationMinutes);

            if (hasConflict)
            {
                throw new InvalidOperationException("Thời gian đặt lịch này đã được đặt hoặc bị trùng với lịch khác.");
            }

            // Validate appointment time is in the future
            if (request.AppointmentTime <= DateTime.UtcNow)
            {
                throw new InvalidOperationException("Thời gian đặt lịch phải trong tương lai.");
            }

            var appointment = new Appointment
            {
                PatientId = request.PatientId,
                DoctorId = request.DoctorId,
                AppointmentTime = request.AppointmentTime,
                DurationMinutes = request.DurationMinutes,
                Status = 0, // Scheduled
                Notes = request.Notes,
                CreatedAt = DateTime.UtcNow
            };

            var createdAppointment = await _appointmentRepository.AddAsync(appointment);
            return createdAppointment.Id;
        }
    }
}


