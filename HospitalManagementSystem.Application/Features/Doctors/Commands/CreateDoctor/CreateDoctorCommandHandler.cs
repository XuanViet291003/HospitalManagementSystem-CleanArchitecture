using HospitalManagementSystem.Core.Entities;
using HospitalManagementSystem.Core.Interfaces.Repositories;
using MediatR;

namespace HospitalManagementSystem.Application.Features.Doctors.Commands.CreateDoctor
{
    public class CreateDoctorCommandHandler : IRequestHandler<CreateDoctorCommand, long>
    {
        private readonly IDoctorRepository _doctorRepository;
        private readonly IUserRepository _userRepository;
        private readonly IDepartmentRepository _departmentRepository;

        public CreateDoctorCommandHandler(
            IDoctorRepository doctorRepository,
            IUserRepository userRepository,
            IDepartmentRepository departmentRepository)
        {
            _doctorRepository = doctorRepository;
            _userRepository = userRepository;
            _departmentRepository = departmentRepository;
        }

        public async Task<long> Handle(CreateDoctorCommand request, CancellationToken cancellationToken)
        {
            // Validate user exists
            var user = await _userRepository.GetByIdAsync(request.UserId);
            if (user == null)
            {
                throw new InvalidOperationException($"Không tìm thấy người dùng với Id = {request.UserId}");
            }

            // Validate department exists
            var department = await _departmentRepository.GetByIdAsync(request.DepartmentId);
            if (department == null)
            {
                throw new InvalidOperationException($"Không tìm thấy phòng ban với Id = {request.DepartmentId}");
            }

            // Check if user already has a doctor profile
            var existingDoctorByUser = await _doctorRepository.GetByUserIdAsync(request.UserId);
            if (existingDoctorByUser != null)
            {
                throw new InvalidOperationException($"Người dùng với Id = {request.UserId} đã có hồ sơ bác sĩ.");
            }

            // Check for duplicate license number (case-insensitive)
            var existingDoctorByLicense = await _doctorRepository.GetByLicenseNumberAsync(request.LicenseNumber);
            if (existingDoctorByLicense != null)
            {
                throw new InvalidOperationException($"LicenseNumber '{request.LicenseNumber}' đã tồn tại.");
            }

            var doctor = new Doctor
            {
                UserId = request.UserId,
                DepartmentId = request.DepartmentId,
                Specialization = request.Specialization,
                LicenseNumber = request.LicenseNumber,
                ConsultationFee = request.ConsultationFee,
                CreatedAt = DateTime.UtcNow
            };

            var created = await _doctorRepository.AddAsync(doctor);
            return created.Id;
        }
    }
}

