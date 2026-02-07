using MediatR;
using HospitalManagementSystem.Application.DTOs.Departments;
using HospitalManagementSystem.Core.Interfaces.Repositories;
using AutoMapper;

namespace HospitalManagementSystem.Application.Features.Departments.Queries.GetByName
{
    public class GetByNameDepartmentQueryHandler : IRequestHandler<GetByNameDepartment, DepartmentDto>
    {
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IMapper _mapper;
        public GetByNameDepartmentQueryHandler(IDepartmentRepository departmentRepository, IMapper mapper)
        {
            _departmentRepository = departmentRepository;
            _mapper = mapper;
        }

        public async Task<DepartmentDto> Handle(GetByNameDepartment request, CancellationToken cancellationToken)
        {
            var departmentFromDb = await _departmentRepository.GetByNameAsync(request.Name);
            var departmentDto = _mapper.Map<DepartmentDto>(departmentFromDb);
            return departmentDto;
        }
    }
}
