using ChatMessages.Application.Contracts;
using ChatMessages.Domain.Entities;
using ChatMessages.Domain.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace ChatMessages.Application.Hubs;

public class ChatHub : Hub
{
    private readonly IUnitOfWork _unitOfWork;

    public ChatHub(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public override async Task OnConnectedAsync()
    {
        var userIdStr = Context.GetHttpContext()?.Request.Query["userId"].ToString();
        if (!int.TryParse(userIdStr, out var userId))
        {
            await base.OnConnectedAsync();
            return;
        }

        await Groups.AddToGroupAsync(Context.ConnectionId, $"user-1");

        var pendingChats = await _unitOfWork.ChatRepository!
            .GetPendingChatsForUserAsync(userId);

        foreach (var chat in pendingChats)
        {
            chat.Status = EChatStatus.Active;
            _unitOfWork.ChatRepository.Update(chat);

            await Clients.User(chat.SenderUserId.ToString())
                .SendAsync("UserAvailable", chat.ReceiverUserId);

            await Clients.User(chat.ReceiverUserId.ToString())
                .SendAsync("ChatStarted", chat.SenderUserId);
        }

        await _unitOfWork.CommitAsync();
        await base.OnConnectedAsync();
    }

    public async Task SendMessage(string receiverUserId, string message, string chatId)
    {
        var senderUserId = Context.GetHttpContext()?.Request.Query["userId"].ToString();

        if (string.IsNullOrEmpty(senderUserId))
            throw new HubException("Usuário não identificado");

        ChatMessage chatMessage = new()
        {
            UserId = int.Parse(senderUserId),
            ChatId = int.Parse(chatId),
            Message = message,
            CreatedAt = DateTime.Now
        };

        await _unitOfWork.ChatMessageRepository!.AddAsync(chatMessage);
        await _unitOfWork.CommitAsync();

        // Enviar mensagem direto para o "usuário"
        await Clients.Group($"user-1")
            .SendAsync("ReceiveMessage", senderUserId, message);
    }

    public async Task RequestChat(int targetUserId)
    {
        var requesterId = GetCurrentUserId();
        if (requesterId == null) return;

        var chat = new Chat
        {
            SenderUserId = requesterId.Value,
            ReceiverUserId = targetUserId,
            Status = EChatStatus.Pending,
            CreatedAt = DateTime.Now
        };

        await _unitOfWork.ChatRepository.AddAsync(chat);
        await _unitOfWork.CommitAsync();

        await Clients.User(requesterId.Value.ToString())
            .SendAsync("WaitingForUser", targetUserId);
    }
    private int? GetCurrentUserId()
    {
        var userIdStr = Context.GetHttpContext()?.Request.Query["userId"].ToString();
        return int.TryParse(userIdStr, out var userId) ? userId : null;
    }

    public async Task RegisterPublicKey(string publicKey, string chatIdString)
    {
        var userIdString = Context.GetHttpContext()?.Request.Query["userId"].ToString();
        int.TryParse(userIdString, out int userId);
        int.TryParse(chatIdString, out int chatId);

        var chatKey = await _unitOfWork.ChatKeyRepository.GetAsync(x => x.ChatId.Equals(chatId) && x.UserId.Equals(userId) && x.Active);
        if (chatKey != null)
        {
            chatKey.Active = false;
            _unitOfWork.ChatKeyRepository.Update(chatKey);
        }

        var newChatKey = new ChatKey
        {
            ChatId = chatId,
            UserId = userId,
            PublicKey = publicKey,
            ExpiresAt = DateTime.Now.AddMinutes(3),
            Active = true
        };
        await _unitOfWork.ChatKeyRepository.AddAsync(newChatKey);
        await _unitOfWork.CommitAsync();
    }

    public async Task<List<UserKey>?> GetPublicKey(string chatIdString)
    {
        int.TryParse(chatIdString, out var chatId);

        var chatKeys = await _unitOfWork.ChatKeyRepository.GetAllAsync(x => x.ChatId.Equals(chatId) && x.Active);
        if (chatKeys == null) 
            return null;

        var response = chatKeys.Select(x => new UserKey()
        {
            PublicKey = x.PublicKey,
            UserId = x.UserId
        }).ToList();

        return response;
    }
}
