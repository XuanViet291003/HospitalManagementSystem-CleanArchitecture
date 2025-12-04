using HospitalManagementSystem.Core.Entities;
using HospitalManagementSystem.Core.Interfaces.Repositories;
using MediatR;
using System.Threading;

namespace HospitalManagementSystem.Application.Features.SystemConfigurations.Commands.UpdateSystemConfiguration
{
    public class UpdateSystemConfigurationCommandHandler : IRequestHandler<UpdateSystemConfigurationCommand>
    {
        private readonly ISystemConfigurationRepository _configRepository;

        public UpdateSystemConfigurationCommandHandler(ISystemConfigurationRepository configRepository)
        {
            _configRepository = configRepository;
        }

        public async Task Handle(UpdateSystemConfigurationCommand request, CancellationToken cancellationToken)
        {
            var config = new SystemConfiguration
            {
                Key = request.Key,
                Value = request.Value,
                Description = request.Description
            };

            await _configRepository.AddOrUpdateAsync(config);
        }
    }
}


