using ChatMessages.Domain.Entities;
using ChatMessages.Domain.Interfaces;
using ChatMessages.Infrastructure.Context;

namespace ChatMessages.Infrastructure.Repositories;

public class ChatRepository : Repository<Chat>, IChatRepository
{
    public ChatRepository(ChatMessageContext context) : base(context)
    {
    }

    public Task<Chat?> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<List<Chat>> GetPendingChatsForUserAsync(int userId)
    {
        var chat = GetAllAsync(x => x.ReceiverUserId.Equals(userId) && 
                                 x.Status.Equals(EChatStatus.Pending));

        return chat;
    }
}
