namespace ChatMessages.Domain.Entities;

public class ChatUser
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int ChatId { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool Accepted { get; set; }
    public Chat? Chat { get; set; }
    public User? User { get; set; }
}
