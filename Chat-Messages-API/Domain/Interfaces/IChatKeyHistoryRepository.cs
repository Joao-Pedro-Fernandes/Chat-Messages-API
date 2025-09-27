using ChatMessages.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatMessages.Domain.Interfaces
{
    public interface IChatKeyHistoryRepository
    {
        Task<ChatKey?> GetByIdAsync(int id);
        Task<IEnumerable<ChatKey>> GetByChatIdAsync(int chatId);
        Task AddAsync(ChatKey history);
    }
}
