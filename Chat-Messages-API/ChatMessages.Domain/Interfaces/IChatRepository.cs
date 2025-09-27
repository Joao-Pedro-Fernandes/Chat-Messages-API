using ChatMessages.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
