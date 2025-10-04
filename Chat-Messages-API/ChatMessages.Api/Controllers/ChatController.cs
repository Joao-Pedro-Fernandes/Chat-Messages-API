using ChatMessages.Application.Contracts;
using ChatMessages.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Chat_Messages_API.Controllers;

[ApiController]
[Route("[controller]")]
public class ChatController : ControllerBase
{
    private readonly ChatService _chatService;

    public ChatController(ChatService chatService)
    {
        _chatService = chatService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateChatAsync([FromBody] PostCreateChatRequest request)
    {
        var chat = await _chatService.CreateChatAsync(request);
        return Ok(chat);
    }

}
