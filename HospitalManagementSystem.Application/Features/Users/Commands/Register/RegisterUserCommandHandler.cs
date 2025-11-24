using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR; 
using System.Threading;
using System.Threading.Tasks;
using HospitalManagementSystem.Core.Entities; 
using HospitalManagementSystem.Core.Interfaces.Repositories; 


namespace HospitalManagementSystem.Application.Features.Users.Commands.Register
{
    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, long>
    {
        
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;

        public RegisterUserCommandHandler(IUserRepository userRepository, IRoleRepository roleRepository)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
        }


        public async Task<long> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var patientRole = await _roleRepository.GetByNameAsync("Patient");
            if (patientRole == null) throw new InvalidOperationException("Role 'Patient' không có trong database.");
            var existingUser = await _userRepository.GetByEmailAsync(request.Email);
            if (existingUser != null)
            {
                throw new System.InvalidOperationException($"Email '{request.Email}' đã được đăng ký.");
            }

            var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

            var newUser = new User
            {
                Email = request.Email,
                PasswordHash = passwordHash,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                RoleId = patientRole.Id
            };

            var createdUser = await _userRepository.AddAsync(newUser);

            return createdUser.Id;
        }
    }
}
