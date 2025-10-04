using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace ChatMessages.Infrastructure.Context;

public class ChatMessageContextFactory : IDesignTimeDbContextFactory<ChatMessageContext>
{
    public ChatMessageContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ChatMessageContext>();
        optionsBuilder.UseMySql(
            "Server=100.81.41.26;Port=3307;Database=ChatDB;User=root;Password=JFYHT#&NUB#&#%;",
            new MySqlServerVersion(new Version(8, 0, 33))
        );

        return new ChatMessageContext(optionsBuilder.Options);
    }
}