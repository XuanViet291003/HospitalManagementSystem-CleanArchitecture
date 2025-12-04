using HospitalManagementSystem.Application.Features.Users.Commands.Register;
using HospitalManagementSystem.Application.Features.Users.Commands.UpdateUser;
using HospitalManagementSystem.Application.Features.Users.Queries.Login;
using MediatR; 
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc; 
using System.Security.Claims;
using System.Threading.Tasks; 

namespace HospitalManagementSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")] 
    public class UsersController : ControllerBase
    {
        
        private readonly IMediator _mediator;
        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        
        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterUserCommand command)
        {
            try
            {
                var userId = await _mediator.Send(command);
                return StatusCode(201, new { UserId = userId });
            }
            catch (InvalidOperationException ex)
            {
                // Bắt lỗi nghiệp vụ cụ thể (ví dụ: email trùng)
                return Conflict(new { Message = ex.Message }); 
            }
            
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserCommand command)
        {
            try
            {
                var userId = await _mediator.Send(command);
                return CreatedAtAction(nameof(GetUserProfile), new { id = userId }, new { UserId = userId });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { Message = ex.Message });

            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginQuery query)
        {
            try
            {
                var loginResponse = await _mediator.Send(query);
                return Ok(loginResponse); 
            }
            catch (Exception ex)
            {
                return Unauthorized(new { Message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        public Task<IActionResult> GetUserById(long id)
        {
             // Sau này sẽ implement Query "GetUserByIdQuery"
             // var user = await _mediator.Send(new GetUserByIdQuery { Id = id });
             // return Ok(user);
             return Task.FromResult<IActionResult>(Ok($"Đây là thông tin của user có id = {id}"));
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")] 
        public async Task<IActionResult> UpdateUser(long id, [FromBody] UpdateUserCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest("ID trong URL và ID trong body của request không khớp.");
            }

            try
            {
                await _mediator.Send(command);

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
    

        [HttpGet("profile")]
        [Authorize] 
        public Task<IActionResult> GetUserProfile()
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userIdString))
            {
                return Task.FromResult<IActionResult>(Unauthorized());
            }

            // Chỗ này sau này mày sẽ tạo một Query "GetUserByIdQuery" để lấy thông tin đầy đủ
            // Bây giờ cứ trả về tạm cái Id và Email để test
            var userEmail = User.FindFirstValue(ClaimTypes.Email);

            return Task.FromResult<IActionResult>(Ok(new { UserId = userIdString, Email = userEmail }));
        }

    }
 
}