using Abp.Domain.Repositories;
using ChatMessages.Domain.Entities;
using ChatMessages.Domain.Interfaces;
using ChatMessages.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatMessages.Infrastructure.Repositories
{
    public class ChatMessageRepository : Repository<ChatMessage>, IChatMessageRepository
    {
        public ChatMessageRepository(ChatMessageContext contex) : base(contex) { }

        public Task<ChatMessage?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
