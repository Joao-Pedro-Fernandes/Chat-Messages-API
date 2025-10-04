using Abp.Domain.Repositories;
using ChatMessages.Domain.Entities;
using ChatMessages.Domain.Interfaces;
using ChatMessages.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatMessages.Infrastructure.Repositories;

public class ChatKeyRepository : Repository<ChatKey>, IChatKeyRepository
{
    public ChatKeyRepository(ChatMessageContext context): base(context)
    {}

    public Task<ChatMessage?> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }
}
