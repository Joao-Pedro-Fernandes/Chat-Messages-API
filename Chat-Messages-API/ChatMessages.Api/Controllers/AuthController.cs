using ChatMessages.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Chat_Messages_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;
        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpGet("get")]
        public async Task<IActionResult> GetUsersAsync()
        {

            var user = await _authService.GetUsersAsync();
            return Ok(user);
        }
    }
}
