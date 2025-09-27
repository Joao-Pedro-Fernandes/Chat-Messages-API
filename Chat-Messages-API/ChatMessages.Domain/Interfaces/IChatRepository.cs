using ChatMessages.Domain.Entities;

namespace ChatMessages.Domain.Interfaces
{
    public interface IChatRepository
    {
        Task<Chat?> GetByIdAsync(int id);
        Task<IEnumerable<Chat>> GetAllAsync();
        Task AddAsync(Chat chat);
        Task UpdateAsync(Chat chat);
        Task DeleteAsync(int id);
    }
}
