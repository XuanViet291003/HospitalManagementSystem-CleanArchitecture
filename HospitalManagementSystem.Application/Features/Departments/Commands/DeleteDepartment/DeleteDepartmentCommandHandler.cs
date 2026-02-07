using HospitalManagementSystem.Core.Interfaces.Repositories;
using MediatR;

namespace HospitalManagementSystem.Application.Features.Departments.Commands.DeleteDepartment
{
    public class DeleteDepartmentCommandHandler : IRequestHandler<DeleteDepartmentCommand>
    {
        private readonly IDepartmentRepository _departmentRepository;

        public DeleteDepartmentCommandHandler(IDepartmentRepository departmentRepository)
        {
            _departmentRepository = departmentRepository;
        }

        public async Task Handle(DeleteDepartmentCommand request, CancellationToken cancellationToken)
        {
            var departmentToDelete = await _departmentRepository.GetByIdAsync(request.Id);
            if (departmentToDelete == null)
            {
                throw new InvalidOperationException($"Không tìm thấy khoa với Id = {request.Id}");
            }
            await _departmentRepository.DeleteAsync(departmentToDelete);
        }
    }
}
