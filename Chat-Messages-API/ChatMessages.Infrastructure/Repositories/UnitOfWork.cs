using ChatMessages.Domain.Interfaces;
using ChatMessages.Infrastructure.Context;

namespace ChatMessages.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly ChatMessageContext _context;

    public UnitOfWork(ChatMessageContext context)
    {
        _context = context;
    }

    private IUserRepository? _userRepository;
    private IChatRepository? _chatRepository;
    private IChatKeyRepository? _chatKeyRepository;
    private IChatMessageRepository? _chatMessageRepository;

    public IUserRepository? UserRepository { get { return _userRepository = _userRepository ?? new UserRepository(_context);  } }
    public IChatRepository? ChatRepository { get { return _chatRepository = _chatRepository ?? new ChatRepository(_context); } }
    public IChatMessageRepository? ChatMessageRepository { get { return _chatMessageRepository = _chatMessageRepository ?? new ChatMessageRepository(_context); } }
    public IChatKeyRepository? ChatKeyRepository { get { return _chatKeyRepository = _chatKeyRepository ?? new ChatKeyRepository(_context); } }

    public Task<int> CommitAsync()
    {
        return _context.SaveChangesAsync();
    }
}
