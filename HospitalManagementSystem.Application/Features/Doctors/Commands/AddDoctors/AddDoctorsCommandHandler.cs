using AutoMapper;
using HospitalManagementSystem.Core.Entities;
using HospitalManagementSystem.Core.Interfaces.Repositories;
using MediatR;

namespace HospitalManagementSystem.Application.Features.Doctors.Commands.AddDoctors
{
    public class AddDoctorsCommandHandler : IRequestHandler<AddDoctorsCommand, long>
    {
        private readonly IDoctorRepository _doctorRepository;
        private readonly IMapper _mapper;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IUserRepository _userRepository;

        public AddDoctorsCommandHandler(
            IDoctorRepository doctorRepository,
            IDepartmentRepository departmentRepository,
            IMapper mapper,
            IUserRepository userRepository)
        {
            _doctorRepository = doctorRepository;
            _mapper = mapper;
            _departmentRepository = departmentRepository;
            _userRepository = userRepository;
        }

        public async Task<long> Handle(AddDoctorsCommand request, CancellationToken cancellationToken)
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

            var newDoctor = _mapper.Map<Doctor>(request);
            newDoctor.CreatedAt = DateTime.UtcNow;
            var doctor = await _doctorRepository.AddAsync(newDoctor);
            return doctor.Id;
        }
    }
}
