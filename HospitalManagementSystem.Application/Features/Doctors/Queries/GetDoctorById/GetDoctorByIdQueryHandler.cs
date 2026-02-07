using AutoMapper;
using HospitalManagementSystem.Application.DTOs.Doctors;
using HospitalManagementSystem.Core.Interfaces.Repositories;
using MediatR;


namespace HospitalManagementSystem.Application.Features.Doctors.Queries.GetDoctorById
{
    public class GetDoctorByIdQueryHandler : IRequestHandler<GetDoctorByIdQuery, DoctorDto>
    {
        private readonly IMapper _mapper;
        private readonly IDoctorRepository _doctorRepository;
        public GetDoctorByIdQueryHandler(IMapper mapper, IDoctorRepository doctorRepository)
        {
            _mapper = mapper;
            _doctorRepository = doctorRepository;
        }
        public async Task<DoctorDto> Handle(GetDoctorByIdQuery request, CancellationToken cancellationToken)
        {
            var doctor = await _doctorRepository.GetByIdAsync(request.Id);
            if (doctor == null)
            {
                throw new InvalidOperationException($"Không tìm thấy bác sĩ với Id = {request.Id}");
            }
            return _mapper.Map<DoctorDto>(doctor);
        }
    }
}
