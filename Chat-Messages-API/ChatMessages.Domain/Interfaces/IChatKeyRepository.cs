using ChatMessages.Domain.Entities;

namespace ChatMessages.Domain.Interfaces
{
    public interface IChatKeyRepository : IRepository<ChatKey>
    {
        Task<ChatMessage?> GetByIdAsync(int id);
    }
}
