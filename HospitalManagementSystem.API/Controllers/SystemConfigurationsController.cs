using HospitalManagementSystem.Application.DTOs.SystemConfigurations;
using HospitalManagementSystem.Application.Features.SystemConfigurations.Commands.UpdateSystemConfiguration;
using HospitalManagementSystem.Application.Features.SystemConfigurations.Queries.GetAllSystemConfigurations;
using HospitalManagementSystem.Application.Features.SystemConfigurations.Queries.GetSystemConfiguration;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HospitalManagementSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SystemConfigurationsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SystemConfigurationsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAll()
        {
            var query = new GetAllSystemConfigurationsQuery();
            var configs = await _mediator.Send(query);
            return Ok(configs);
        }

        [HttpGet("{key}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetByKey(string key)
        {
            var query = new GetSystemConfigurationQuery { Key = key };
            var config = await _mediator.Send(query);
            if (config == null)
            {
                return NotFound(new { Message = $"Không tìm thấy cấu hình với key = {key}" });
            }
            return Ok(config);
        }

        [HttpPut("{key}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(string key, [FromBody] UpdateSystemConfigurationRequest request)
        {
            var command = new UpdateSystemConfigurationCommand
            {
                Key = key,
                Value = request.Value,
                Description = request.Description
            };
            await _mediator.Send(command);
            return NoContent();
        }
    }

    public class UpdateSystemConfigurationRequest
    {
        public required string Value { get; set; }
        public string? Description { get; set; }
    }
}


