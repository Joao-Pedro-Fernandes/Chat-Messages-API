namespace ChatMessages.Application.Contracts;

public class GetChatKeysResponse
{
    List<UserKey> UserKeys { get; set; } = new();
}

public class UserKey
{
    public int UserId { get; set; }
    public required string PublicKey { get; set; }
}