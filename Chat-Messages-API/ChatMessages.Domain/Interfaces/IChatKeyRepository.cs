using ChatMessages.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatMessages.Domain.Interfaces
{
    public interface IChatKeyRepository : IRepository<ChatKey>
    {
        Task<ChatMessage?> GetByIdAsync(int id);
    }
}
