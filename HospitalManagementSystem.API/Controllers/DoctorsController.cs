using HospitalManagementSystem.Application.Features.Doctors.Queries.GetDoctorsByDepartment;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HospitalManagementSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public DoctorsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("department/{departmentId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetDoctorsByDepartment(long departmentId)
        {
            var query = new GetDoctorsByDepartmentQuery { DepartmentId = departmentId };
            var doctors = await _mediator.Send(query);
            return Ok(doctors);
        }
    }
}


