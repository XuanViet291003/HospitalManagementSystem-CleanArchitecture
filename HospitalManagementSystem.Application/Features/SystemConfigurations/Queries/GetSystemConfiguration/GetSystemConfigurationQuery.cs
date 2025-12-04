using HospitalManagementSystem.Application.DTOs.SystemConfigurations;
using MediatR;

namespace HospitalManagementSystem.Application.Features.SystemConfigurations.Queries.GetSystemConfiguration
{
    public class GetSystemConfigurationQuery : IRequest<SystemConfigurationDto?>
    {
        public required string Key { get; set; }
    }
}


