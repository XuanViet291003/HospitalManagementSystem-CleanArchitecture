using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HospitalManagementSystem.Core.Interfaces.Repositories;
using HospitalManagementSystem.Core.Entities;
using MediatR;

namespace HospitalManagementSystem.Application.Features.Departments.Commands.UpdateDepartment
{
    public class UpdateDepartmentCommandHandler : IRequestHandler<UpdateDepartmentCommand>
    {
        private readonly IDepartmentRepository _departmentRepository;

        public UpdateDepartmentCommandHandler(IDepartmentRepository departmentRepository)
        {
            _departmentRepository = departmentRepository;
        }

        public async Task Handle(UpdateDepartmentCommand request, CancellationToken cancellationToken)
        {
            var departmentToUpdate = await _departmentRepository.GetByIdAsync(request.Id);
            if (departmentToUpdate == null)
            {
                throw new Exception($"Không tìm thấy khoa với Id = {request.Id}");
            }

            if (departmentToUpdate.Name.ToLower() != request.Name.ToLower())
            {
                var existingDepartment = await _departmentRepository.GetByNameAsync(request.Name);
                if (existingDepartment != null && existingDepartment.Id != request.Id)
                {
                    throw new Exception($"Khoa với tên '{request.Name}' đã tồn tại.");
                }
            }
            departmentToUpdate.Name = request.Name;
            departmentToUpdate.Description = request.Description;
            departmentToUpdate.UpdatedAt = DateTime.UtcNow;

            await _departmentRepository.UpdateAsync(departmentToUpdate);
        }

    }
}
