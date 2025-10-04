using ChatMessages.Application.Contracts;
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

    public async Task<User> RegisterAsync(PostRegisterRequest request)
    {
        User user = new()
        {
            CreatedAt = DateTime.UtcNow,
            LastAcessAt = DateTime.UtcNow,
            Name = request.Name,
            Password = request.Password
        };

        await _unitOfWork.UserRepository.AddAsync(user);
        await _unitOfWork.CommitAsync();

        return user;
    }

    public async Task<User?> LoginAsync(PostRegisterRequest request)
    {
        var user = await _unitOfWork.UserRepository.GetAsync(u => u.Name == request.Name && u.Password == request.Password);

        if (user == null)
        {
            return null;
        }   

        return user;
    }
}
