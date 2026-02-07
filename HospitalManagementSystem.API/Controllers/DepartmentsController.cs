using HospitalManagementSystem.Application.Features.Departments.Commands.CreateDepartment;
using HospitalManagementSystem.Application.Features.Departments.Commands.DeleteDepartment;
using HospitalManagementSystem.Application.Features.Departments.Commands.UpdateDepartment;
using HospitalManagementSystem.Application.Features.Departments.Queries.GetAllDepartments;
using HospitalManagementSystem.Application.Features.Departments.Queries.GetDepartmentById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace HospitalManagementSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentsController : ControllerBase
    {
        private readonly IMediator _mediator; 
        public DepartmentsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateDepartment([FromBody] CreateDepartmentCommand command)
        {
            var departmentId = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetDepartmentById), 
            new { id = departmentId }, new { DepartmentId = departmentId });
        }


        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetDepartmentById(long id)
        {
            try
            {
                var query = new GetDepartmentByIdQuery { Id = id };
                var department = await _mediator.Send(query);
                return Ok(department);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
        }

        [HttpGet] 
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _mediator.Send(new GetAllDepartmentsQuery()));
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(long id, [FromBody] UpdateDepartmentCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest("ID trong URL không khớp với ID trong body.");
            }
            await _mediator.Send(command);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(long id)
        {
            var command = new DeleteDepartmentCommand { Id = id };
            await _mediator.Send(command);
            return NoContent();
        }
    }
}
