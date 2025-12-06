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


namespace HospitalManagementSystem.Application.Features.Doctors.Queries.GetAllDoctor
{
    public class GetAllDoctorQueryHandler : IRequestHandler<GetAllDoctorQuery, List<DoctorDto>>
    {
        private readonly IDoctorRepository _doctorRepository;
        private readonly IMapper _mapper;
        public GetAllDoctorQueryHandler(IDoctorRepository doctorRepository, IMapper mapper)
        {
            _doctorRepository = doctorRepository;
            _mapper = mapper;
        }
        public async Task<List<DoctorDto>> Handle(GetAllDoctorQuery request, CancellationToken cancellationToken)
        {
            var doctors = await _doctorRepository.GetAllAsync();
            return _mapper.Map<List<DoctorDto>>(doctors);
        }

    }
}
