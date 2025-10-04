namespace ChatMessages.Domain.Entities;

public class ChatKey
{
    public int Id { get; set; }

    public int ChatId { get; set; }
    public int UserId { get; set; }
    public string PublicKey { get; set; }
    public DateTime ExpiresAt { get; set; }
    public bool Active { get; set; }
    public DateTime CreatedAt { get; set; }

    public Chat Chat { get; set; }
    public User User { get; set; }
}
