using HospitalManagementSystem.Application.DTOs.Users;
using HospitalManagementSystem.Core.Interfaces.Repositories;
using HospitalManagementSystem.Core.Interfaces.Services;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagementSystem.Application.Features.Users.Queries.Login
{
    public class LoginQueryHandler : IRequestHandler<LoginQuery, LoginResponseDto>
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;

        public LoginQueryHandler(IUserRepository userRepo, IJwtTokenGenerator jwtGen)
        {
            _userRepository = userRepo;
            _jwtTokenGenerator = jwtGen;
        }

        public async Task<LoginResponseDto> Handle(LoginQuery request, CancellationToken cancellationToken)
        {
            // 1. Tìm user theo email
            var user = await _userRepository.GetByEmailAsync(request.Email);
            if (user == null)
            {
                throw new Exception("Sai thông tin đăng nhập.");
            }

            // 2. Verify mật khẩu 
            var isPasswordValid = BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash);
            if (!isPasswordValid)
            {
                throw new Exception("Sai thông tin đăng nhập.");
            }

            // 3. Tạo token 
            var tokenString = _jwtTokenGenerator.GenerateToken(user);

            // 4. Tạo đối tượng DTO và trả về
            var response = new LoginResponseDto
            {
                Token = tokenString
            };

            return response;
        }
    }
}
