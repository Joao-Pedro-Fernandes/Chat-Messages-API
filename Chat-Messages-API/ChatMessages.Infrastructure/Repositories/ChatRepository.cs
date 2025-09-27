using ChatMessages.Domain.Entities;
using ChatMessages.Domain.Interfaces;
using ChatMessages.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ChatMessages.Infrastructure.Repositories
{
    public class ChatRepository : Repository<Chat>, IChatRepository
    {
        public ChatRepository(ChatMessageContext context) : base(context) { }

        public Task<Chat?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
