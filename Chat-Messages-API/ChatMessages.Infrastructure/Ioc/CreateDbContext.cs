using ChatMessages.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace ChatMessages.Infrastructure.Context
{
    public class ChatMessageContextFactory : IDesignTimeDbContextFactory<ChatMessageContext>
    {
        public ChatMessageContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ChatMessageContext>();
            optionsBuilder.UseMySql(
                "Server=localhost;Database=ChatDB;User=root;Password=28072002;",
                new MySqlServerVersion(new Version(8, 0, 33))
            );

            return new ChatMessageContext(optionsBuilder.Options);
        }
    }
}