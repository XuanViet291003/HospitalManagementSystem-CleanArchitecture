using HospitalManagementSystem.Application.DTOs.SystemConfigurations;
using MediatR;

namespace HospitalManagementSystem.Application.Features.SystemConfigurations.Queries.GetAllSystemConfigurations
{
    public class GetAllSystemConfigurationsQuery : IRequest<List<SystemConfigurationDto>>
    {
    }
}


