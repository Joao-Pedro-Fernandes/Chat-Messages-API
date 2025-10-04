namespace ChatMessages.Application.Contracts;

public class PostLoginRequest
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
