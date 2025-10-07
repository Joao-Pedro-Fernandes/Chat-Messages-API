using ChatMessages.Application.Contracts;
using ChatMessages.Domain.Entities;
using ChatMessages.Domain.Interfaces;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace ChatMessages.Application.Hubs;

public class ChatHub : Hub
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<ChatHub> _logger;

    public ChatHub(IUnitOfWork unitOfWork, ILogger<ChatHub> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public override async Task OnConnectedAsync()
    {
        _logger.LogInformation("## Solicitação de conexão recebida, ConnectionId: " + Context.ConnectionId);
        var userIdString = Context.GetHttpContext()?.Request.Query["userId"].ToString();
        int.TryParse(userIdString, out var userId);

        var pendingChatIds = await _unitOfWork.ChatUserRepository.GetPendingChatsForUserAsync(userId);

        var pendingChats = await _unitOfWork.ChatRepository.GetAllAsync(x => pendingChatIds.Contains(x.Id) && x.Status == EChatStatus.Pending);

        foreach (var chat in pendingChats)
        {
            _logger.LogInformation($"## Notificando convite de chat para o usuário {userId.ToString()}");
            await Clients.User(userId.ToString())
                .SendAsync("NotifyReceiver", chat.CreatorUserId, chat.Id);
        }

        await base.OnConnectedAsync();
    }

    public async Task NotifyKeyUpdate(int chatId)
    {
        var userIdString = Context.GetHttpContext()?.Request.Query["userId"].ToString();
        int.TryParse(userIdString, out var userId);

        var chat = await _unitOfWork.ChatRepository.GetAsync(x => x.Id.Equals(chatId));
        if (chat == null)
            return;

        var chatOtherUsers = await _unitOfWork.ChatUserRepository.GetAsync(x => x.ChatId.Equals(chatId) && x.UserId != userId);
        if (chatOtherUsers == null)
            return;

        await Clients.User(chatOtherUsers.UserId.ToString())
            .SendAsync("NotificationUpdatedKeys");
    }

    public async Task SendMessage(int chatId, string message)
    {
        _logger.LogInformation("## SendMessage, ConnectionId: " + Context.ConnectionId);
        var senderUserId = Context.GetHttpContext()?.Request.Query["userId"].ToString();
        _logger.LogInformation("## [UserId]" + senderUserId);
        _logger.LogInformation("## [Message]" + message);
        _logger.LogInformation("## [ChatId]" + chatId);
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

    public async Task<object?> JoinChat(int userId, int otherUserId, int? chatId, bool? accepted)
    {
        _logger.LogInformation("## JoinChat, ConnectionId: " + Context.ConnectionId);
        Chat? chat;

        if (chatId.HasValue && chatId > 0)
        {
            chat = await _unitOfWork.ChatRepository
                .GetAsync(x => x.Id.Equals(chatId.Value));
            if (chat == null)
                return null;

            _logger.LogInformation("## [UserId]" + userId.ToString());
            _logger.LogInformation("## [ChaId]" + chatId.ToString());

            if (chat.Status == EChatStatus.Blocked)
                return null;

            if (chat.Status == EChatStatus.Active)
            {
                _logger.LogInformation("## [Group] ChaId: " + chatId.ToString() + "UserId: " + userId);
                await Groups.AddToGroupAsync(Context.ConnectionId, $"{chatId}");
                return null;
            }   

            if (accepted != null && !accepted.Value)
            {
                chat.Status = EChatStatus.Blocked;
                _unitOfWork.ChatRepository.Update(chat);
                await _unitOfWork.CommitAsync();
                await Clients.User(chat.CreatorUserId.ToString())
                    .SendAsync("NotificationRefused", chat.Id, chat.Status.ToString());
                return new
                {
                    chat.Id,
                    Status = chat.Status.ToString()
                };
            }

            var userInChat = await _unitOfWork.ChatUserRepository
                .GetAsync(x => x.ChatId.Equals(chat.Id) && x.UserId.Equals(userId));
            if (userInChat == null)
                return null;

            if (!userInChat.Accepted)
            {
                userInChat.Accepted = true;
                chat.Status = EChatStatus.Active;
                _unitOfWork.ChatRepository.Update(chat);
                _unitOfWork.ChatUserRepository.Update(userInChat);
                await _unitOfWork.CommitAsync();

                _logger.LogInformation("## [Group] ChaId: " + chatId.ToString() + "UserId: " + userId);
                await Groups.AddToGroupAsync(Context.ConnectionId, $"{chatId}");

                await Clients.User(chat.CreatorUserId.ToString())
                    .SendAsync("NotificationAccepted", chat.Id, chat.Status.ToString());

                return new
                {
                    chat.Id,
                    Status = chat.Status.ToString()
                };
            }

            if (userInChat.Accepted)
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, $"{chatId}");
                _logger.LogInformation("## [Group] ChaId: " + chatId.ToString() + "UserId: " + userId);
            }
            
            return new
            {
                chat.Id,
                Status = chat.Status.ToString()
            };
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

        _logger.LogInformation($"## Notificando convite de chat para o usuário {userId.ToString()}");
        await Clients.User(otherUserId.ToString())
            .SendAsync("NotifyReceiver", userId, chat.Id);

        await Groups.AddToGroupAsync(Context.ConnectionId, $"{chat.Id}");
        _logger.LogInformation("## [Group] ChaId: " + chat.Id.ToString() + "UserId: " + userId);

        return new
        {
            chat.Id,
            Status = chat.Status.ToString()
        };
    }

    public async Task RegisterPublicKey(string publicKey, int chatId)
    {
        _logger.LogInformation("## RegisterPublicKey, ConnectionId: " + Context.ConnectionId);
        var userIdString = Context.GetHttpContext()?.Request.Query["userId"].ToString();
        _logger.LogInformation("## [UserId]" + userIdString);
        _logger.LogInformation("## [ChatId]" + chatId);
        int.TryParse(userIdString, out int userId);

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

    public async Task<object?> GetPublicKey(int chatId)
    {
        _logger.LogInformation("## GetPublicKey, ConnectionId: " + Context.ConnectionId);
        var userIdString = Context.GetHttpContext()?.Request.Query["userId"].ToString();
        int.TryParse(userIdString, out int userId);

        var chatKeys = await _unitOfWork.ChatKeyRepository.GetAllAsync(x => x.ChatId.Equals(chatId) && x.Active);
        if (chatKeys == null) 
            return null;

        var otherChatKey = chatKeys.FirstOrDefault(x => !x.UserId.Equals(userId));
        var userChatKey = chatKeys.FirstOrDefault(x => !x.UserId.Equals(userId));
        if (otherChatKey == null || userChatKey == null)
            return null;

        var response = new
        {
            UserId = userId,
            UserPublicKey = userChatKey.PublicKey,
            OtherUserId = otherChatKey.UserId,
            OtherUserPublicKey = otherChatKey.PublicKey,
        };

        return response;
    }
}
