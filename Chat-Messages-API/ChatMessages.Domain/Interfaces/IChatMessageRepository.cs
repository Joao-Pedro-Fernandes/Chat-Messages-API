using ChatMessages.Domain.Entities;

namespace ChatMessages.Domain.Interfaces;

public interface IChatMessageRepository : IRepository<ChatMessage>
{
    Task<ChatMessage?> GetByIdAsync(int id);
}
