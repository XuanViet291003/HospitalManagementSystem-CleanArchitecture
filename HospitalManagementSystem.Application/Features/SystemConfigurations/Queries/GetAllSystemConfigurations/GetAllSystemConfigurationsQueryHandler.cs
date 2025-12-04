using AutoMapper;
using HospitalManagementSystem.Application.DTOs.SystemConfigurations;
using HospitalManagementSystem.Core.Interfaces.Repositories;
using MediatR;
using System.Threading;

namespace HospitalManagementSystem.Application.Features.SystemConfigurations.Queries.GetAllSystemConfigurations
{
    public class GetAllSystemConfigurationsQueryHandler : IRequestHandler<GetAllSystemConfigurationsQuery, List<SystemConfigurationDto>>
    {
        private readonly ISystemConfigurationRepository _configRepository;
        private readonly IMapper _mapper;

        public GetAllSystemConfigurationsQueryHandler(ISystemConfigurationRepository configRepository, IMapper mapper)
        {
            _configRepository = configRepository;
            _mapper = mapper;
        }

        public async Task<List<SystemConfigurationDto>> Handle(GetAllSystemConfigurationsQuery request, CancellationToken cancellationToken)
        {
            var configs = await _configRepository.GetAllAsync();
            return _mapper.Map<List<SystemConfigurationDto>>(configs);
        }
    }
}


