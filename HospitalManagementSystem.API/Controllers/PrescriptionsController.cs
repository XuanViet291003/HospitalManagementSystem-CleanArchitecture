using HospitalManagementSystem.Application.DTOs.Prescriptions;
using HospitalManagementSystem.Application.Features.Prescriptions.Commands.CreatePrescription;
using HospitalManagementSystem.Application.Features.Prescriptions.Queries.GetPrescriptionById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HospitalManagementSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PrescriptionsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PrescriptionsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Authorize(Roles = "Doctor,Admin")]
        public async Task<IActionResult> CreatePrescription([FromBody] CreatePrescriptionDto dto)
        {
            try
            {
                var command = new CreatePrescriptionCommand
                {
                    MedicalRecordId = dto.MedicalRecordId,
                    Items = dto.Items.Select(item => new PrescriptionItemRequest
                    {
                        MedicineId = item.MedicineId,
                        Quantity = item.Quantity,
                        Dosage = item.Dosage,
                        Notes = item.Notes
                    }).ToList()
                };

                var prescriptionId = await _mediator.Send(command);
                return CreatedAtAction(nameof(GetPrescriptionById), new { id = prescriptionId }, new { PrescriptionId = prescriptionId });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetPrescriptionById(long id)
        {
            try
            {
                var query = new GetPrescriptionByIdQuery { Id = id };
                var prescription = await _mediator.Send(query);
                return Ok(prescription);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
        }
    }
}


