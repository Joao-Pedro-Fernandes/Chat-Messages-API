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
        var userIdString = Context.GetHttpContext()?.Request.Query["userId"].ToString();
        int.TryParse(userIdString, out var userId);

        var pendingChatIds = await _unitOfWork.ChatUserRepository.GetPendingChatsForUserAsync(userId);

        var pendingChats = await _unitOfWork.ChatRepository.GetAllAsync(x => pendingChatIds.Contains(x.Id) && x.Status == EChatStatus.Pending);

        foreach (var chat in pendingChats)
        {
            await Clients.User(userId.ToString())
                .SendAsync("NotifyReceiver", chat.CreatorUserId, chat.Id);
        }

        await base.OnConnectedAsync();
    }

    public async Task SendMessage(int chatId, string message)
    {
        var senderUserId = Context.GetHttpContext()?.Request.Query["userId"].ToString();
        if (string.IsNullOrEmpty(senderUserId))
            throw new HubException("Usuário não identificado");

        ChatMessage chatMessage = new()
        {
            UserId = int.Parse(senderUserId),
            ChatId = chatId,
            Message = message,
            CreatedAt = DateTime.Now
        };

        await _unitOfWork.ChatMessageRepository!.AddAsync(chatMessage);
        await _unitOfWork.CommitAsync();

        await Clients.Group($"{chatId}")
            .SendAsync("ReceiveMessage", senderUserId, message);
    }

    public async Task JoinChat(int userId, int otherUserId, int? chatId, bool? accepted)
    {
        Chat? chat;

        if (chatId.HasValue && chatId > 0)
        {
            chat = await _unitOfWork.ChatRepository
                .GetAsync(x => x.Id.Equals(chatId.Value));
            if (chat == null)
                return;

            if (chat.Status == EChatStatus.Blocked)
                return;

            if (chat.Status == EChatStatus.Active)
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, $"{chatId}");
                return;
            }   

            if (accepted != null && !accepted.Value)
            {
                chat.Status = EChatStatus.Blocked;
                await _unitOfWork.CommitAsync();
                await Clients.User(chat.CreatorUserId.ToString())
                    .SendAsync("NotificationRefused");
                return;
            }

            var userInChat = await _unitOfWork.ChatUserRepository
                .GetAsync(x => x.ChatId.Equals(chat.Id) && x.UserId.Equals(userId));
            if (userInChat == null)
                return;

            if (!userInChat.Accepted)
            {
                userInChat.Accepted = true;
                chat.Status = EChatStatus.Active;
                _unitOfWork.ChatRepository.Update(chat);
                _unitOfWork.ChatUserRepository.Update(userInChat);
                await _unitOfWork.CommitAsync();

                await Groups.AddToGroupAsync(Context.ConnectionId, $"{chatId}");

                await Clients.User(chat.CreatorUserId.ToString())
                    .SendAsync("NotificationAccepted");

                return;
            }

            if (userInChat.Accepted)
                await Groups.AddToGroupAsync(Context.ConnectionId, $"{chatId}");
            
            return;
        }

        chat = new Chat
        {
            CreatorUserId = userId,
            Status = EChatStatus.Pending,
            CreatedAt = DateTime.Now
        };
        await _unitOfWork.ChatRepository.AddAsync(chat);
        await _unitOfWork.CommitAsync();

        await _unitOfWork.ChatUserRepository.AddAsync(new ChatUser
        {
            ChatId = chat.Id,
            UserId = userId,
            Accepted = true,
            CreatedAt = DateTime.Now,
        });

        await _unitOfWork.ChatUserRepository.AddAsync(new ChatUser
        {
            ChatId = chat.Id,
            UserId = otherUserId,
            Accepted = false,
            CreatedAt = DateTime.Now,
        });
        await _unitOfWork.CommitAsync();

        await Clients.User(userId.ToString())
            .SendAsync("NotifyReceiver", userId, chatId);
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
