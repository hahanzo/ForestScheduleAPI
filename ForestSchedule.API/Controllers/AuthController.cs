using ForestSchedule.Application.DTOs.AuthDtos;
using ForestSchedule.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ForestSchedule.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController(IAuthService authService) : ControllerBase
    {
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            try
            {
                var result = await authService.RegisterAsync(dto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var result = await authService.LoginAsync(dto);
            if (result == null)
                return Unauthorized(new { Message = "Incorrect email or password." });

            return Ok(result);
        }
    }
}
