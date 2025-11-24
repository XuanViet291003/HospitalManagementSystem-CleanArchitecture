using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using HospitalManagementSystem.Application.DTOs.Departments;
using HospitalManagementSystem.Core.Interfaces.Repositories;

namespace HospitalManagementSystem.Application.Features.Departments.Queries.GetAllDepartments
{
    public class GetAllDepartmentsQueryHandler : IRequestHandler<GetAllDepartmentsQuery, List<DepartmentDto>>
    {
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IMapper _mapper;

        public GetAllDepartmentsQueryHandler(IDepartmentRepository departmentRepository, IMapper mapper)
        {
            _departmentRepository = departmentRepository;
            _mapper = mapper;
        }

        public async Task<List<DepartmentDto>> Handle(GetAllDepartmentsQuery request, CancellationToken cancellationToken)
        {
            var departmentsFromDb = await _departmentRepository.GetAllAsync();
            
            var departmentsDto = _mapper.Map<List<DepartmentDto>>(departmentsFromDb);

            return departmentsDto;
        }
    }
}
