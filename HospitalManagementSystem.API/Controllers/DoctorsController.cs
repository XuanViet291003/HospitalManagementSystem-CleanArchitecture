using HospitalManagementSystem.Application.Features.Doctors.Commands.CreateDoctor;
using HospitalManagementSystem.Application.Features.Doctors.Commands.UpdateDoctor;
using HospitalManagementSystem.Application.Features.Doctors.Queries.GetAllDoctor;
using HospitalManagementSystem.Application.Features.Doctors.Queries.GetDoctorById;
using HospitalManagementSystem.Application.Features.Doctors.Queries.GetDoctorsByDepartment;
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

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            var query = new GetAllDoctorQuery();
            var doctors = await _mediator.Send(query);
            return Ok(doctors);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(long id)
        {
            try
            {
                var query = new GetDoctorByIdQuery { Id = id };
                var doctor = await _mediator.Send(query);
                return Ok(doctor);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
        }

        [HttpGet("department/{departmentId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetDoctorsByDepartment(long departmentId)
        {
            var query = new GetDoctorsByDepartmentQuery { DepartmentId = departmentId };
            var doctors = await _mediator.Send(query);
            return Ok(doctors);
        }

        [HttpGet("user/{userId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetByUserId(long userId)
        {
            try
            {
                var query = new GetDoctorsByUsersQuery { UserId = userId };
                var doctor = await _mediator.Send(query);
                if (doctor == null)
                {
                    return NotFound(new { Message = $"Không tìm thấy bác sĩ với UserId = {userId}" });
                }
                return Ok(doctor);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] CreateDoctorCommand command)
        {
            try
            {
                var doctorId = await _mediator.Send(command);
                return CreatedAtAction(nameof(GetById), new { id = doctorId }, new { DoctorId = doctorId });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(long id, [FromBody] UpdateDoctorCommand command)
        {
            try
            {
                if (id != command.Id)
                {
                    return BadRequest(new { Message = "ID trong URL không khớp với ID trong body." });
                }

                var doctor = await _mediator.Send(command);
                return Ok(doctor);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
    }
}


