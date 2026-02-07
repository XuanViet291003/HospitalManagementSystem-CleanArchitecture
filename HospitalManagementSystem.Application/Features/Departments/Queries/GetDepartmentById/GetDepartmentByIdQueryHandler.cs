using AutoMapper;
using HospitalManagementSystem.Application.DTOs.Departments;
using HospitalManagementSystem.Core.Interfaces.Repositories;
using MediatR;

namespace HospitalManagementSystem.Application.Features.Departments.Queries.GetDepartmentById
{
    public class GetDepartmentByIdQueryHandler : IRequestHandler<GetDepartmentByIdQuery, DepartmentDto>
    {
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IMapper _mapper;

        public GetDepartmentByIdQueryHandler(IDepartmentRepository departmentRepository, IMapper mapper)
        {
            _departmentRepository = departmentRepository;
            _mapper = mapper;
        }

        public async Task<DepartmentDto> Handle(GetDepartmentByIdQuery request, CancellationToken cancellationToken)
        {
            var department = await _departmentRepository.GetByIdAsync(request.Id);
            if (department == null)
            {
                throw new InvalidOperationException($"Không tìm thấy phòng ban với Id = {request.Id}");
            }
            return _mapper.Map<DepartmentDto>(department);
        }
    }
}

