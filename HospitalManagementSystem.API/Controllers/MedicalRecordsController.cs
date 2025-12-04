using HospitalManagementSystem.Application.DTOs.MedicalRecords;
using HospitalManagementSystem.Application.Features.MedicalRecords.Commands.CreateMedicalRecord;
using HospitalManagementSystem.Application.Features.MedicalRecords.Commands.UpdateMedicalRecord;
using HospitalManagementSystem.Application.Features.MedicalRecords.Queries.GetMedicalRecordById;
using HospitalManagementSystem.Application.Features.MedicalRecords.Queries.GetMedicalRecordsByPatientId;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HospitalManagementSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MedicalRecordsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public MedicalRecordsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Authorize(Roles = "Doctor,Admin")]
        public async Task<IActionResult> CreateMedicalRecord([FromBody] CreateMedicalRecordDto dto)
        {
            try
            {
                var command = new CreateMedicalRecordCommand
                {
                    AppointmentId = dto.AppointmentId,
                    Symptoms = dto.Symptoms,
                    Diagnosis = dto.Diagnosis,
                    TreatmentPlan = dto.TreatmentPlan,
                    FollowUpDate = dto.FollowUpDate
                };

                var medicalRecordId = await _mediator.Send(command);
                return CreatedAtAction(nameof(GetMedicalRecordById), new { id = medicalRecordId }, new { MedicalRecordId = medicalRecordId });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetMedicalRecordById(long id)
        {
            try
            {
                var query = new GetMedicalRecordByIdQuery { Id = id };
                var medicalRecord = await _mediator.Send(query);
                return Ok(medicalRecord);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
        }

        [HttpGet("patient/{patientId}")]
        [Authorize]
        public async Task<IActionResult> GetMedicalRecordsByPatientId(long patientId)
        {
            var query = new GetMedicalRecordsByPatientIdQuery { PatientId = patientId };
            var medicalRecords = await _mediator.Send(query);
            return Ok(medicalRecords);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Doctor,Admin")]
        public async Task<IActionResult> UpdateMedicalRecord(long id, [FromBody] UpdateMedicalRecordDto dto)
        {
            try
            {
                var command = new UpdateMedicalRecordCommand
                {
                    Id = id,
                    Symptoms = dto.Symptoms,
                    Diagnosis = dto.Diagnosis,
                    TreatmentPlan = dto.TreatmentPlan,
                    FollowUpDate = dto.FollowUpDate
                };

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


