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

    public async Task<List<GetChatUsersResponse>?> GetChatUsersAsync(int id)
    {
        var users = await _unitOfWork.UserRepository.GetAllAsync(u => u.Id != id);
        var chatUsers = await _unitOfWork.ChatUserRepository.GetAllAsync(c => c.UserId != id);
        var chatIds = chatUsers.Select(u => u.ChatId).ToList();
        var chats = await _unitOfWork.ChatRepository.GetAllAsync(c => chatIds.Contains(c.Id));

        List<GetChatUsersResponse> chatsResponse = new List<GetChatUsersResponse>();

        foreach (var user in users)
        {
            var chatUser = chatUsers.FirstOrDefault(cu => cu.UserId == user.Id);
            var chat = chats.FirstOrDefault(c => c.Id == chatUser?.ChatId);


            GetChatUsersResponse addChat = new()
            {
                OtherUserId = user.Id,
                OtherUserName = user.Name,
                ChatId = chat?.Id ?? 0,
                Accepted = chatUser?.Accepted ?? false,
                StatusChat = chat?.Status.ToString() ?? null
            };

            chatsResponse.Add(addChat);
          
        }   

        return chatsResponse;   
    }
}
