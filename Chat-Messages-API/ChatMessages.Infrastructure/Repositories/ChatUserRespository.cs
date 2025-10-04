using ChatMessages.Domain.Entities;
using ChatMessages.Domain.Interfaces;
using ChatMessages.Infrastructure.Context;

namespace ChatMessages.Infrastructure.Repositories;

public class ChatUserRespository : Repository<ChatUser>, IChatUserRepository
{
    public ChatUserRespository(ChatMessageContext context) : base(context)
    {
    }

    public async Task<List<int>> GetPendingChatsForUserAsync(int userId)
    {
        var chatUsers = await GetAllAsync(x => x.UserId.Equals(userId) && !x.Accepted);

        return chatUsers.Select(x => x.ChatId).ToList();
    }
}
