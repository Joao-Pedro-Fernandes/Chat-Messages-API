namespace ChatMessages.Domain.Interfaces; 
public interface IUnitOfWork
{
    IChatUserRepository ChatUserRepository { get; }
    IUserRepository UserRepository { get; }
    IChatRepository ChatRepository { get; }
    IChatMessageRepository ChatMessageRepository { get; }
    IChatKeyRepository ChatKeyRepository { get; }
    Task<int> CommitAsync(); 
} 