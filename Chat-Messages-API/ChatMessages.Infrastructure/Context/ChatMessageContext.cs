using ChatMessages.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ChatMessages.Infrastructure.Context;

public class ChatMessageContext : DbContext
{
    public ChatMessageContext(DbContextOptions<ChatMessageContext> options)
    : base(options)
    { }


    public DbSet<User> Users { get; set; }
    public DbSet<ChatMessage> ChatMessages { get; set; }
    public DbSet<ChatKey> ChatKeys { get; set; }
    public DbSet<Chat> Chats { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(u  => u.Id);
        });

        modelBuilder.Entity<Chat>(entity =>
        {
            entity.HasKey(c => c.Id);

            entity.HasOne(c => c.SenderUser)
                  .WithMany()
                  .HasForeignKey(c => c.SenderUserId);

            entity.HasOne(c => c.ReceiverUser)
                  .WithMany()
                  .HasForeignKey(c => c.ReceiverUserId);

            entity.Property(c => c.Status)
                .HasConversion<string>();
        });

        modelBuilder.Entity<ChatMessage>(entity =>
        {
            entity.HasKey(cm => cm.Id);

            entity.HasOne(c => c.User)
                  .WithMany(x => x.Messages)
                  .HasForeignKey(cm => cm.UserId);

            entity.HasOne(c => c.Chat)
                  .WithMany(x => x.Messages)
                  .HasForeignKey(cm => cm.ChatId);
        });

        modelBuilder.Entity<ChatKey>(entity =>
        {
            entity.HasKey(ck => ck.Id);

            entity.HasOne(c => c.User)
                  .WithMany()
                  .HasForeignKey(ck => ck.UserId);

            entity.HasOne(c => c.Chat)
                  .WithMany()
                  .HasForeignKey(ck => ck.ChatId);
        });
    }
}
