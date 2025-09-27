using ChatMessages.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ChatMessages.Infrastructure.Context
{
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

                entity.HasOne<User>()
                      .WithMany()
                      .HasForeignKey(c => c.SenderUserId);

                entity.HasOne<User>()
                      .WithMany()
                      .HasForeignKey(c => c.ReceiverUserId);
            });

            modelBuilder.Entity<ChatMessage>(entity =>
            {
                entity.HasKey(cm => cm.Id);

                entity.HasOne<User>()
                      .WithMany()
                      .HasForeignKey(cm => cm.UserId);

                entity.HasOne<Chat>()
                      .WithMany()
                      .HasForeignKey(cm => cm.ChatId);
            });


            modelBuilder.Entity<ChatKey>(entity =>
            {
                entity.HasKey(ck => ck.Id);

                entity.HasOne<User>()
                      .WithMany()
                      .HasForeignKey(ck => ck.UserId);

                entity.HasOne<Chat>()
                      .WithMany()
                      .HasForeignKey(ck => ck.ChatId);
            });

        }




    }
}
