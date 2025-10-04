using System.Threading.Tasks;
namespace ChatMessages.Domain.Interfaces; 
public interface IUnitOfWork
{
    IUserRepository UserRepository { get; }
    IChatRepository ChatRepository { get; }
    IChatMessageRepository ChatMessageRepository { get; }
    IChatKeyRepository ChatKeyRepository { get; }
    Task<int> CommitAsync(); 
} 