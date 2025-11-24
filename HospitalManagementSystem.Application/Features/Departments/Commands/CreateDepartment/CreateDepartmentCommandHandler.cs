using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HospitalManagementSystem.Core.Interfaces.Repositories;
using HospitalManagementSystem.Core.Entities;


namespace HospitalManagementSystem.Application.Features.Departments.Commands.CreateDepartment
{
    public class CreateDepartmentCommandHandler : IRequestHandler<CreateDepartmentCommand, long>
    {
        private readonly IDepartmentRepository _departmentRepository;
        public CreateDepartmentCommandHandler(IDepartmentRepository departmentRepository)
        {
            _departmentRepository = departmentRepository;
        }

        public async Task<long> Handle(CreateDepartmentCommand request, CancellationToken cancellationToken)
        {
            var existingDepartment = await _departmentRepository.GetByNameAsync(request.Name);
            if (existingDepartment != null)
            {
                throw new InvalidOperationException($"Khoa '{request.Name}' đã tồn tại.");
            }
            var newDepartment = new Department
            {
                Name = request.Name,
                Description = request.Description,
                CreatedAt = DateTime.UtcNow
            };
            var createdDepartment = await _departmentRepository.AddAsync(newDepartment);
            return createdDepartment.Id;
        }

    }
}
