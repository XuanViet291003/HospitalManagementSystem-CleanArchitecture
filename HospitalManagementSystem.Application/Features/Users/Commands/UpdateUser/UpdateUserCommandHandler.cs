using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using System.Threading;
using HospitalManagementSystem.Core.Entities;
using HospitalManagementSystem.Core.Interfaces.Repositories;


namespace HospitalManagementSystem.Application.Features.Users.Commands.UpdateUser
{
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand>
    {
        private readonly IUserRepository _userRepository;

        public UpdateUserCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var userToUpdate = await _userRepository.GetByIdAsync(request.Id);
            if (userToUpdate == null)
            {
                throw new Exception($"Không tìm thấy user với Id = {request.Id}");
            }

            // Kiểm tra email trùng lặp (nếu email có thay đổi)
            if (userToUpdate.Email.ToLower() != request.Email.ToLower())
            {
                var existingUser = await _userRepository.GetByEmailAsync(request.Email);
                if (existingUser != null && existingUser.Id != request.Id)
                {
                    throw new InvalidOperationException($"Email '{request.Email}' đã được sử dụng.");
                }
            }

            
            //var roleExists = await _roleRepository.GetByIdAsync(request.RoleId);
            //if (roleExists == null) throw new Exception("RoleId không hợp lệ.");

            
            userToUpdate.Email = request.Email;
            userToUpdate.IsActive = request.IsActive;
            userToUpdate.RoleId = request.RoleId;
            userToUpdate.UpdatedAt = DateTime.UtcNow;

            await _userRepository.UpdateAsync(userToUpdate);
        }
    }
}
