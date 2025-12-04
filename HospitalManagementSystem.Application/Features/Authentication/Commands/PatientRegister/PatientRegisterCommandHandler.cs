using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HospitalManagementSystem.Core.Interfaces.Repositories;
using HospitalManagementSystem.Core.Entities;
using MediatR;

namespace HospitalManagementSystem.Application.Features.Authentication.Commands.PatientRegister
{
    public class PatientRegisterCommandHandler : IRequestHandler <PatientRegisterCommand, long>
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        public PatientRegisterCommandHandler(IUserRepository userRepository, IRoleRepository roleRepository)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
        }
        public async Task<long> Handle(PatientRegisterCommand request, CancellationToken cancellationToken)
        {
            var existingUser = await _userRepository.GetByEmailAsync(request.Email);
            if (existingUser != null)
            {
                throw new InvalidOperationException($"Email '{request.Email}' đã được đăng ký.");
            }
            var patientRole = await _roleRepository.GetByNameAsync("Patient");
            if (patientRole == null)
            {
                throw new InvalidOperationException($"Không tìm thấy vai trò mặc định 'Patient'");
            }
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
            var newUser = new User
            {
                Email = request.Email,
                PasswordHash = passwordHash,
                RoleId = patientRole.Id 
            };
            var createdUser = await _userRepository.AddAsync(newUser);
            return createdUser.Id;
        }
    }
}
