using AutoMapper;
using HospitalManagementSystem.Application.DTOs.Doctors;
using HospitalManagementSystem.Core.Interfaces.Repositories;
using MediatR;
using System.Threading;

namespace HospitalManagementSystem.Application.Features.Doctors.Queries.GetDoctorsByDepartment
{
    public class GetDoctorsByDepartmentQueryHandler : IRequestHandler<GetDoctorsByDepartmentQuery, List<DoctorDto>>
    {
        private readonly IDoctorRepository _doctorRepository;
        private readonly IMapper _mapper;

        public GetDoctorsByDepartmentQueryHandler(IDoctorRepository doctorRepository, IMapper mapper)
        {
            _doctorRepository = doctorRepository;
            _mapper = mapper;
        }

        public async Task<List<DoctorDto>> Handle(GetDoctorsByDepartmentQuery request, CancellationToken cancellationToken)
        {
            var doctors = await _doctorRepository.GetByDepartmentIdAsync(request.DepartmentId);
            return _mapper.Map<List<DoctorDto>>(doctors);
        }
    }
}


