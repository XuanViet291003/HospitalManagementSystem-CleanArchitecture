using AutoMapper;
using HospitalManagementSystem.Application.DTOs.SystemConfigurations;
using HospitalManagementSystem.Core.Interfaces.Repositories;
using MediatR;
using System.Threading;

namespace HospitalManagementSystem.Application.Features.SystemConfigurations.Queries.GetSystemConfiguration
{
    public class GetSystemConfigurationQueryHandler : IRequestHandler<GetSystemConfigurationQuery, SystemConfigurationDto?>
    {
        private readonly ISystemConfigurationRepository _configRepository;
        private readonly IMapper _mapper;

        public GetSystemConfigurationQueryHandler(ISystemConfigurationRepository configRepository, IMapper mapper)
        {
            _configRepository = configRepository;
            _mapper = mapper;
        }

        public async Task<SystemConfigurationDto?> Handle(GetSystemConfigurationQuery request, CancellationToken cancellationToken)
        {
            var config = await _configRepository.GetByKeyAsync(request.Key);
            return config != null ? _mapper.Map<SystemConfigurationDto>(config) : null;
        }
    }
}


