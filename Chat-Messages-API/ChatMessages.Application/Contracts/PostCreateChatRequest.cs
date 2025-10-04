namespace ChatMessages.Application.Contracts;

public class PostCreateChatRequest
{
    public int SenderUserId { get; set; }
    public int ReceiverUserId { get; set; }
}
