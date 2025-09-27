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
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(ChatMessageContext context) : base(context)
        {
            
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            return null;
        }

        public async Task<IEnumerable<User>> GetUsers()
        {
            var users = await GetAllAsync();
            return users;
        }

    }
}
