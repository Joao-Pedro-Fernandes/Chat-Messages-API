namespace ChatMessages.Domain.Entities;

public class Chat
{
    public int Id { get; set; }
    public int CreatorUserId { get; set; }
    public EChatStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public User CreatorUser { get; set; }
    public ICollection<ChatMessage> Messages { get; set; } 
}

public enum EChatStatus
{
    Pending,
    Active,
    Blocked
}
