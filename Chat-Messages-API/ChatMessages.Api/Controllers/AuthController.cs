using ChatMessages.Application.Contracts;
using ChatMessages.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Chat_Messages_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;
        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] PostLoginRequest request)
        {
            _authService.
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
