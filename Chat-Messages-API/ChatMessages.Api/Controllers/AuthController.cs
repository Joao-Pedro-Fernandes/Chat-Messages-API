using ChatMessages.Application.Contracts;
using ChatMessages.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Chat_Messages_API.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly AuthService _authService;
    public AuthController(AuthService authService)
    {
        _authService = authService;
    }

    [HttpGet("chat-users")]
    public async Task<IActionResult> GetChatUsersAsync([FromQuery]int id)
    {
        var user = await _authService.GetChatUsersAsync(id);
        return Ok(user);
    }

    [HttpPost("register")]
    public async Task<IActionResult> RegisterAsync([FromBody] PostRegisterRequest request)
    {
        var user = await _authService.RegisterAsync(request);
        return Ok(user);
    }

    [HttpPost("login")]
    public async Task<IActionResult> LoginAsync([FromBody] PostRegisterRequest request)
    {
        var user = await _authService.LoginAsync(request);
        if (user == null)
        {
            return Unauthorized("Invalid credentials");
        }

        return Ok(user);
    }
}
