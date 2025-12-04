using HospitalManagementSystem.Application.DTOs.Payments;
using HospitalManagementSystem.Application.Features.Payments.Commands.CreatePayment;
using HospitalManagementSystem.Application.Features.Payments.Queries.GetPaymentsByInvoiceId;
using HospitalManagementSystem.Application.Features.Payments.Queries.GetRevenueReport;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HospitalManagementSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PaymentsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Receptionist")]
        public async Task<IActionResult> CreatePayment([FromBody] CreatePaymentDto dto)
        {
            try
            {
                var command = new CreatePaymentCommand
                {
                    InvoiceId = dto.InvoiceId,
                    Amount = dto.Amount,
                    PaymentMethod = dto.PaymentMethod,
                    TransactionCode = dto.TransactionCode
                };

                var paymentId = await _mediator.Send(command);
                return CreatedAtAction(nameof(GetPaymentsByInvoiceId), new { invoiceId = dto.InvoiceId }, new { PaymentId = paymentId });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpGet("invoice/{invoiceId}")]
        [Authorize]
        public async Task<IActionResult> GetPaymentsByInvoiceId(long invoiceId)
        {
            var query = new GetPaymentsByInvoiceIdQuery { InvoiceId = invoiceId };
            var payments = await _mediator.Send(query);
            return Ok(payments);
        }

        [HttpGet("revenue")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetRevenueReport([FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
        {
            var query = new GetRevenueReportQuery
            {
                StartDate = startDate,
                EndDate = endDate
            };
            var report = await _mediator.Send(query);
            return Ok(report);
        }
    }
}


