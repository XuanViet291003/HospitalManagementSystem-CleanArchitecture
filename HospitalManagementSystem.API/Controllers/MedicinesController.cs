using HospitalManagementSystem.Application.Features.Medicines.Queries.GetMedicines;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HospitalManagementSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MedicinesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public MedicinesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetMedicines([FromQuery] string? searchTerm = null)
        {
            var query = new GetMedicinesQuery { SearchTerm = searchTerm };
            var medicines = await _mediator.Send(query);
            return Ok(medicines);
        }
    }
}


