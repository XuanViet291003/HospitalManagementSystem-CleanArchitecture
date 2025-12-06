using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using HospitalManagementSystem.Application.DTOs.Doctors;
using HospitalManagementSystem.Core.Interfaces.Repositories;
using MediatR;
using System.Threading;

namespace HospitalManagementSystem.Application.Features.Doctors.Queries.GetDoctorsByUsers
{
    public class GetDoctorsByUsersHandler : IRequestHandler<GetDoctorsByUsersQuery, List<DoctorDto>>
    {
        private readonly IMapper _mapper;
        private readonly IDoctorRepository _doctorRepository;
        public GetDoctorsByUsersHandler(IMapper mapper, IDoctorRepository doctorRepository)
        {
            _mapper = mapper;
            _doctorRepository = doctorRepository;
        }
        public async Task<List<DoctorDto>> Handle(GetDoctorsByUsersQuery request, CancellationToken cancellationToken)
        {
            var doctors = await _doctorRepository.GetByUserIdAsync(request.UserId);
            return _mapper.Map<List<DoctorDto>>(doctors);
        }
    }
}
