using ChatMessages.Domain.Entities;

namespace ChatMessages.Domain.Interfaces
{
    public interface IChatMessageRepository
    {
        Task<ChatMessage?> GetByIdAsync(int id);
        Task<IEnumerable<ChatMessage>> GetByChatIdAsync(int chatId);
        Task AddAsync(ChatMessage message);
        Task DeleteAsync(int id);
    }
}
