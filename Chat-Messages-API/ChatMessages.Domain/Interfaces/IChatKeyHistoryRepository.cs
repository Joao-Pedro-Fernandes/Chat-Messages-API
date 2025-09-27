using ChatMessages.Domain.Entities;

namespace ChatMessages.Domain.Interfaces
{
    public interface IChatKeyHistoryRepository
    {
        Task<ChatKey?> GetByIdAsync(int id);
        Task<IEnumerable<ChatKey>> GetByChatIdAsync(int chatId);
        Task AddAsync(ChatKey history);
    }
}
