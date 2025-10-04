namespace ChatMessages.Domain.Entities;

public class ChatMessage
{
    public int Id { get; set; }

    public int UserId { get; set; }
    public int ChatId { get; set; }

    public string Message { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }

    public Chat Chat { get; set; }
    public User User { get; set; }
}
