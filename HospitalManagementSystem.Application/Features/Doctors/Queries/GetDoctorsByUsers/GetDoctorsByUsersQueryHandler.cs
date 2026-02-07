using AutoMapper;
using HospitalManagementSystem.Application.DTOs.Doctors;
using HospitalManagementSystem.Core.Interfaces.Repositories;
using MediatR;

namespace HospitalManagementSystem.Application.Features.Doctors.Queries.GetDoctorsByUsers
{
    public class GetDoctorsByUsersHandler : IRequestHandler<GetDoctorsByUsersQuery, DoctorDto?>
    {
        private readonly IMapper _mapper;
        private readonly IDoctorRepository _doctorRepository;

        public GetDoctorsByUsersHandler(IMapper mapper, IDoctorRepository doctorRepository)
        {
            _mapper = mapper;
            _doctorRepository = doctorRepository;
        }

        public async Task<DoctorDto?> Handle(GetDoctorsByUsersQuery request, CancellationToken cancellationToken)
        {
            var doctor = await _doctorRepository.GetByUserIdAsync(request.UserId);
            if (doctor == null)
            {
                return null;
            }
            return _mapper.Map<DoctorDto>(doctor);
        }
    }
}
