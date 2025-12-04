using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using HospitalManagementSystem.Core.Entities;
using HospitalManagementSystem.Core.Interfaces.Repositories;
using HospitalManagementSystem.Core.Interfaces.Services;
using MediatR;

namespace HospitalManagementSystem.Application.Features.Users.Commands.Register
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, long>
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IEmailService _emailService; 

        public CreateUserCommandHandler(IUserRepository userRepository, IRoleRepository roleRepository, IEmailService emailService)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _emailService = emailService;
        }

        public async Task<long> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var existingUser = await _userRepository.GetByEmailAsync(request.Email);

            if (existingUser != null) 
            {
                throw new InvalidOperationException($"Email'{request.Email}' đã tồn tại.");
            }


            var userRole = await _roleRepository.GetByNameAsync(request.RoleName);

            if(userRole == null)
            {
                throw new InvalidOperationException($"Role'{request.RoleName}' không hợp lệ.");
            }


            var temporaryPassword = GenerateRandomPassword();
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(temporaryPassword);

            var newUser = new User
            {
                Email = request.Email,
                PasswordHash = passwordHash,
                RoleId = userRole.Id,
                IsActive = true, 
                CreatedAt = DateTime.UtcNow
            };

            var createdUser = await _userRepository.AddAsync(newUser);

            await _emailService.SendAccountActivationEmailAsync(createdUser.Email, temporaryPassword);

            return createdUser.Id;

        }
        private string GenerateRandomPassword(int length = 12)
        {
            const string validChars = "ABCDEFGHJKLMNOPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz0123456789!@#$%^&*?";
            var random = new Random();
            var password = new char[length];
            for (int i = 0; i < length; i++)
            {
                password[i] = validChars[random.Next(validChars.Length)];
            }
            return new string(password);
        }
    }
}
