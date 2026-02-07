using HospitalManagementSystem.Core.Interfaces.Repositories;
using MediatR;
using HospitalManagementSystem.Application.DTOs.Doctors;
using AutoMapper;
namespace HospitalManagementSystem.Application.Features.Doctors.Commands.UpdateDoctor
{
    public class UpdateDoctorCommandHandler : IRequestHandler<UpdateDoctorCommand, DoctorDto>
    {
        private readonly IDoctorRepository _doctorRepository;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IMapper _mapper;

        public UpdateDoctorCommandHandler(
            IDoctorRepository doctorRepository,
            IDepartmentRepository departmentRepository,
            IMapper mapper)
        {
            _doctorRepository = doctorRepository;
            _departmentRepository = departmentRepository;
            _mapper = mapper;
        }

        public async Task<DoctorDto> Handle(UpdateDoctorCommand request, CancellationToken cancellationToken)
        {
            var doctor = await _doctorRepository.GetByIdAsync(request.Id);
            if (doctor == null)
            {
                throw new InvalidOperationException($"Không tìm thấy bác sĩ với Id = {request.Id}");
            }

            // Validate department exists if updating
            if (request.DepartmentId.HasValue)
            {
                var department = await _departmentRepository.GetByIdAsync(request.DepartmentId.Value);
                if (department == null)
                {
                    throw new InvalidOperationException($"Không tìm thấy phòng ban với Id = {request.DepartmentId.Value}");
                }
                doctor.DepartmentId = request.DepartmentId.Value;
            }

            // Validate license number is not duplicate if updating
            if (request.LicenseNumber != null)
            {
                var existingDoctorByLicense = await _doctorRepository.GetByLicenseNumberAsync(request.LicenseNumber);
                if (existingDoctorByLicense != null && existingDoctorByLicense.Id != request.Id)
                {
                    throw new InvalidOperationException($"LicenseNumber '{request.LicenseNumber}' đã được sử dụng bởi bác sĩ khác.");
                }
                doctor.LicenseNumber = request.LicenseNumber;
            }

            // Update only if value is provided
            if (request.Specialization != null)
                doctor.Specialization = request.Specialization;
            
            if (request.ConsultationFee.HasValue)
                doctor.ConsultationFee = request.ConsultationFee.Value;

            doctor.UpdatedAt = DateTime.UtcNow;

            await _doctorRepository.UpdateAsync(doctor);
            return _mapper.Map<DoctorDto>(doctor);
        }
    }
}