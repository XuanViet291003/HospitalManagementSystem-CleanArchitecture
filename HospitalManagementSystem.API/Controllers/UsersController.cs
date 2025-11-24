using HospitalManagementSystem.Application.Features.Users.Commands.Register;
using HospitalManagementSystem.Application.Features.Users.Queries.Login;
using MediatR; 
using Microsoft.AspNetCore.Mvc; 
using System.Threading.Tasks; 
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

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
        public async Task<IActionResult> Register([FromBody] RegisterUserCommand command)
        {
            var userId = await _mediator.Send(command);

            return StatusCode(201, new { UserId = userId });
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

        [HttpGet("profile")]
        [Authorize] 
        public async Task<IActionResult> GetUserProfile()
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier); // Lấy Id từ token

            if (string.IsNullOrEmpty(userIdString))
            {
                return Unauthorized();
            }

            // Chỗ này sau này mày sẽ tạo một Query "GetUserByIdQuery" để lấy thông tin đầy đủ
            // Bây giờ cứ trả về tạm cái Id và Email để test
            var userEmail = User.FindFirstValue(ClaimTypes.Email);

            return Ok(new { UserId = userIdString, Email = userEmail });
        }

    }
}