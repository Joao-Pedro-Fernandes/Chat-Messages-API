using ChatMessages.Domain.Entities;

namespace ChatMessages.Domain.Interfaces
{
    public interface IChatRepository : IRepository<Chat>
    {
        Task<Chat?> GetByIdAsync(int id);

    }
}
