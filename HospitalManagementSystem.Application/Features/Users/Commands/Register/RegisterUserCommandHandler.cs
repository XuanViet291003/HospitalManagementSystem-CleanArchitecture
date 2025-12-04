using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR; 
using System.Threading;
using HospitalManagementSystem.Core.Entities; 
using HospitalManagementSystem.Core.Interfaces.Repositories; 


namespace HospitalManagementSystem.Application.Features.Users.Commands.Register
{
    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, long>
    {
        
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IPatientRepository _patientRepository;

        public RegisterUserCommandHandler(IUserRepository userRepository, IRoleRepository roleRepository, IPatientRepository patientRepository)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _patientRepository = patientRepository;
        }


        public async Task<long> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var existingUser = await _userRepository.GetByEmailAsync(request.Email);
            if (existingUser != null)
            {
                throw new System.InvalidOperationException($"Email '{request.Email}' đã được đăng ký.");
            }

            var userRole = await _roleRepository.GetByNameAsync(request.RoleName);

            if (userRole == null)
            {
                throw new InvalidOperationException($"Role '{request.RoleName}' không hợp lệ");
            }

            var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

            var newUser = new User
            {
                Email = request.Email,
                PasswordHash = passwordHash,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                RoleId = userRole.Id
            };

            var createdUser = await _userRepository.AddAsync(newUser);

            // Auto-create Patient if role is Patient
            if (request.RoleName == "Patient")
            {
                var patientCode = await _patientRepository.GeneratePatientCodeAsync();
                var patient = new Patient
                {
                    UserId = createdUser.Id,
                    PatientCode = patientCode,
                    CreatedAt = DateTime.UtcNow
                };
                await _patientRepository.AddAsync(patient);
            }

            return createdUser.Id;
        }
    }
}
