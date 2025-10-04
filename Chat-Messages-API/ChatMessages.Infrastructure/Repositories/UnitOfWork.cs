using ChatMessages.Domain.Entities;
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

    private IChatUserRepository? _chatUserRepository;
    private IUserRepository? _userRepository;
    private IChatRepository? _chatRepository;
    private IChatKeyRepository? _chatKeyRepository;
    private IChatMessageRepository? _chatMessageRepository;

    public IUserRepository UserRepository =>  
        _userRepository ??= new UserRepository(_context);
    public IChatRepository ChatRepository => 
        _chatRepository ??= new ChatRepository(_context);
    public IChatMessageRepository ChatMessageRepository =>
        _chatMessageRepository ??= new ChatMessageRepository(_context);
    public IChatKeyRepository ChatKeyRepository =>
        _chatKeyRepository ??= new ChatKeyRepository(_context);
    public IChatUserRepository ChatUserRepository =>
        _chatUserRepository ??= new ChatUserRespository(_context);

    public Task<int> CommitAsync()
    {
        return _context.SaveChangesAsync();
    }
}
