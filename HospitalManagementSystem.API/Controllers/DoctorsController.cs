using HospitalManagementSystem.Application.Features.Doctors.Queries.GetAllDoctor;
using HospitalManagementSystem.Application.Features.Doctors.Queries.GetDoctorsByDepartment;
using HospitalManagementSystem.Application.Features.Doctors.Queries.GetDoctorById;
using HospitalManagementSystem.Application.Features.Doctors.Queries.GetDoctorsByUsers;
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

        [HttpGet("users/{userId}")]
        public async Task<IActionResult> GetDoctorsByUsers(long userId)
        {
            var query = new GetDoctorsByUsersQuery { UserId = userId };
            var doctors = await _mediator.Send(query);
            return Ok(doctors);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllDoctor(long id)
        {
            var query = new GetAllDoctorQuery();
            var doctors = await _mediator.Send(query);
            return Ok(doctors);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetDoctorById(long id)
        {
            var query = new GetDoctorByIdQuery { Id = id };
            var doctor = await _mediator.Send(query);
            return Ok(doctor);
        }
    }
}


