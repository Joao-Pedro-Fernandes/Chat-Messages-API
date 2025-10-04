
using ChatMessages.Application.Contracts;
using ChatMessages.Domain.Entities;
using ChatMessages.Domain.Interfaces;

namespace ChatMessages.Application.Services;

public class ChatService
{
    private readonly IUnitOfWork _unitOfWork;

    public ChatService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Chat> CreateChatAsync(PostCreateChatRequest request)
    {
        Chat chat = new Chat()
        {
            CreatedAt = DateTime.Now,
            ReceiverUserId = request.ReceiverUserId,
            SenderUserId = request.SenderUserId,
        };
        await _unitOfWork.ChatRepository.AddAsync(chat);
        await _unitOfWork.CommitAsync();

        return chat;
    }
}
