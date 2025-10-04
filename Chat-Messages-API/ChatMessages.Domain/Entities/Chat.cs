namespace ChatMessages.Domain.Entities;

public class Chat
{
    public int Id { get; set; }
    public int SenderUserId { get; set; }
    public int ReceiverUserId { get; set; }
    public EChatStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public User SenderUser { get; set; }
    public User ReceiverUser { get; set; }
    public ICollection<ChatMessage> Messages { get; set; } 
}

public enum EChatStatus
{
    Pending,
    Active,
    Closed
}
