using ChatMessages.Domain.Entities;
using ChatMessages.Domain.Interfaces;
using ChatMessages.Infrastructure.Context;

namespace ChatMessages.Infrastructure.Repositories;

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
