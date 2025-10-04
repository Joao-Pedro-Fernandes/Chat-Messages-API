using ChatMessages.Domain.Entities;

namespace ChatMessages.Domain.Interfaces;

public interface IChatUserRepository : IRepository<ChatUser>
{
    Task<List<int>> GetPendingChatsForUserAsync(int userId);
}
