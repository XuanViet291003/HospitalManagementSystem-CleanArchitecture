using HospitalManagementSystem.Application.DTOs.Appointments;
using HospitalManagementSystem.Application.Features.Appointments.Commands.CancelAppointment;
using HospitalManagementSystem.Application.Features.Appointments.Commands.CheckInAppointment;
using HospitalManagementSystem.Application.Features.Appointments.Commands.CompleteAppointment;
using HospitalManagementSystem.Application.Features.Appointments.Commands.CreateAppointment;
using HospitalManagementSystem.Application.Features.Appointments.Queries.GetAvailableTimeSlots;
using HospitalManagementSystem.Application.Features.Appointments.Queries.GetPatientAppointments;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HospitalManagementSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AppointmentsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("doctor/{doctorId}/available-slots")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAvailableTimeSlots(long doctorId, [FromQuery] DateTime date, [FromQuery] int slotDurationMinutes = 30)
        {
            var query = new GetAvailableTimeSlotsQuery
            {
                DoctorId = doctorId,
                Date = date,
                SlotDurationMinutes = slotDurationMinutes
            };
            var slots = await _mediator.Send(query);
            return Ok(slots);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateAppointment([FromBody] CreateAppointmentDto dto, [FromServices] HospitalManagementSystem.Core.Interfaces.Repositories.IPatientRepository patientRepository)
        {
            try
            {
                var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userIdString) || !long.TryParse(userIdString, out var userId))
                {
                    return Unauthorized(new { Message = "Không thể xác định người dùng." });
                }

                // Get Patient by UserId
                var patient = await patientRepository.GetByUserIdAsync(userId);
                if (patient == null)
                {
                    return BadRequest(new { Message = "Người dùng này chưa có hồ sơ bệnh nhân. Vui lòng tạo hồ sơ bệnh nhân trước." });
                }

                var command = new CreateAppointmentCommand
                {
                    PatientId = patient.Id,
                    DoctorId = dto.DoctorId,
                    AppointmentTime = dto.AppointmentTime,
                    DurationMinutes = dto.DurationMinutes,
                    Notes = dto.Notes
                };

                var appointmentId = await _mediator.Send(command);
                return CreatedAtAction(nameof(GetAppointmentById), new { id = appointmentId }, new { AppointmentId = appointmentId });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetAppointmentById(long id)
        {
            try
            {
                var query = new Application.Features.Appointments.Queries.GetAppointmentById.GetAppointmentByIdQuery { Id = id };
                var appointment = await _mediator.Send(query);
                return Ok(appointment);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
        }

        [HttpGet("patient/{patientId}")]
        [Authorize]
        public async Task<IActionResult> GetPatientAppointments(long patientId)
        {
            var query = new GetPatientAppointmentsQuery { PatientId = patientId };
            var appointments = await _mediator.Send(query);
            return Ok(appointments);
        }

        [HttpPost("{id}/cancel")]
        [Authorize]
        public async Task<IActionResult> CancelAppointment(long id, [FromBody] CancelAppointmentRequest? request = null)
        {
            try
            {
                var command = new CancelAppointmentCommand
                {
                    AppointmentId = id,
                    Reason = request?.Reason
                };
                await _mediator.Send(command);
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpPost("{id}/check-in")]
        [Authorize(Roles = "Admin,Receptionist")]
        public async Task<IActionResult> CheckInAppointment(long id)
        {
            try
            {
                var command = new CheckInAppointmentCommand { AppointmentId = id };
                await _mediator.Send(command);
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpPost("{id}/complete")]
        [Authorize(Roles = "Doctor,Admin")]
        public async Task<IActionResult> CompleteAppointment(long id)
        {
            try
            {
                var command = new CompleteAppointmentCommand { AppointmentId = id };
                await _mediator.Send(command);
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
    }

    public class CancelAppointmentRequest
    {
        public string? Reason { get; set; }
    }
}

