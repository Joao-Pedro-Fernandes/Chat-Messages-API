using ChatMessages.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
