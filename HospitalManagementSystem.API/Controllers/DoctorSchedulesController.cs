using HospitalManagementSystem.Application.Features.DoctorSchedules.Commands.CreateDoctorSchedule;
using HospitalManagementSystem.Application.Features.DoctorSchedules.Commands.DeleteDoctorSchedule;
using HospitalManagementSystem.Application.Features.DoctorSchedules.Commands.UpdateDoctorSchedule;
using HospitalManagementSystem.Application.Features.DoctorSchedules.Queries.GetDoctorScheduleById;
using HospitalManagementSystem.Application.Features.DoctorSchedules.Queries.GetDoctorSchedules;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HospitalManagementSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorSchedulesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public DoctorSchedulesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Doctor")]
        public async Task<IActionResult> Create([FromBody] CreateDoctorScheduleCommand command)
        {
            try
            {
                var scheduleId = await _mediator.Send(command);
                return CreatedAtAction(nameof(GetById), new { id = scheduleId }, new { ScheduleId = scheduleId });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(long id)
        {
            try
            {
                var query = new GetDoctorScheduleByIdQuery { Id = id };
                var schedule = await _mediator.Send(query);
                return Ok(schedule);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
        }

        [HttpGet("doctor/{doctorId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetByDoctorId(long doctorId, [FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
        {
            var query = new GetDoctorSchedulesQuery
            {
                DoctorId = doctorId,
                StartDate = startDate,
                EndDate = endDate
            };
            var schedules = await _mediator.Send(query);
            return Ok(schedules);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Doctor")]
        public async Task<IActionResult> Update(long id, [FromBody] UpdateDoctorScheduleCommand command)
        {
            try
            {
                if (id != command.Id)
                {
                    return BadRequest(new { Message = "ID trong URL không khớp với ID trong body." });
                }

                await _mediator.Send(command);
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,Doctor")]
        public async Task<IActionResult> Delete(long id)
        {
            try
            {
                var command = new DeleteDoctorScheduleCommand { Id = id };
                await _mediator.Send(command);
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
    }
}

