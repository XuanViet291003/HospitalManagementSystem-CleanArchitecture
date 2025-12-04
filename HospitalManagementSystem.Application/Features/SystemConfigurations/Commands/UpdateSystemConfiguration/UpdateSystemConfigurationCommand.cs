using MediatR;

namespace HospitalManagementSystem.Application.Features.SystemConfigurations.Commands.UpdateSystemConfiguration
{
    public class UpdateSystemConfigurationCommand : IRequest
    {
        public required string Key { get; set; }
        public required string Value { get; set; }
        public string? Description { get; set; }
    }
}


