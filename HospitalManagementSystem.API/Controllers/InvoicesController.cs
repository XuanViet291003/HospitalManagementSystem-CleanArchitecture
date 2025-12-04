using HospitalManagementSystem.Application.DTOs.Invoices;
using HospitalManagementSystem.Application.Features.Invoices.Commands.AddInvoiceItem;
using HospitalManagementSystem.Application.Features.Invoices.Commands.CreateInvoice;
using HospitalManagementSystem.Application.Features.Invoices.Queries.GetInvoiceById;
using HospitalManagementSystem.Application.Features.Invoices.Queries.GetInvoicesByPatientId;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HospitalManagementSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvoicesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public InvoicesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Receptionist")]
        public async Task<IActionResult> CreateInvoice([FromBody] CreateInvoiceDto dto)
        {
            try
            {
                var command = new CreateInvoiceCommand
                {
                    AppointmentId = dto.AppointmentId,
                    Items = dto.Items?.Select(item => new InvoiceItemRequest
                    {
                        ItemDescription = item.ItemDescription,
                        ItemType = item.ItemType,
                        Quantity = item.Quantity,
                        UnitPrice = item.UnitPrice
                    }).ToList(),
                    DueDate = dto.DueDate
                };

                var invoiceId = await _mediator.Send(command);
                return CreatedAtAction(nameof(GetInvoiceById), new { id = invoiceId }, new { InvoiceId = invoiceId });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetInvoiceById(long id)
        {
            try
            {
                var query = new GetInvoiceByIdQuery { Id = id };
                var invoice = await _mediator.Send(query);
                return Ok(invoice);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
        }

        [HttpGet("patient/{patientId}")]
        [Authorize]
        public async Task<IActionResult> GetInvoicesByPatientId(long patientId)
        {
            var query = new GetInvoicesByPatientIdQuery { PatientId = patientId };
            var invoices = await _mediator.Send(query);
            return Ok(invoices);
        }

        [HttpPost("{invoiceId}/items")]
        [Authorize(Roles = "Admin,Receptionist")]
        public async Task<IActionResult> AddInvoiceItem(long invoiceId, [FromBody] AddInvoiceItemDto dto)
        {
            try
            {
                var command = new AddInvoiceItemCommand
                {
                    InvoiceId = invoiceId,
                    ItemDescription = dto.ItemDescription,
                    ItemType = dto.ItemType,
                    Quantity = dto.Quantity,
                    UnitPrice = dto.UnitPrice
                };

                var itemId = await _mediator.Send(command);
                return CreatedAtAction(nameof(GetInvoiceById), new { id = invoiceId }, new { InvoiceItemId = itemId });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
    }
}


