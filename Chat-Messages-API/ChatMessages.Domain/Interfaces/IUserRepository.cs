using ChatMessages.Domain.Entities;

namespace ChatMessages.Domain.Interfaces
{
    public interface IUserRepository: IRepository<User>
    {
        Task<User?> GetByIdAsync(int id);
    }
}
