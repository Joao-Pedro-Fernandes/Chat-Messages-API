using ChatMessages.Domain.Entities;
using ChatMessages.Domain.Interfaces;

namespace ChatMessages.Application.Services;

public class AuthService
{
    private readonly IUnitOfWork _unitOfWork;

    public AuthService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<List<User>> GetUsersAsync()
    {
        var users = await _unitOfWork.UserRepository.GetAllAsync();
        return users;
    }
}
