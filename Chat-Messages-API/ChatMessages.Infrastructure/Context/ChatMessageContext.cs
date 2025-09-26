using ChatMessages.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatMessages.Infrastructure.Context
{
    public class ChatMessageContext : DbContext
    {
        public ChatMessageContext(DbContextOptions<ChatMessageContext> options)
        : base(options)
        { }


        public DbSet<User> Users { get; set; }
        public DbSet<ChatMessage> ChatMessages { get; set; }
        public DbSet<ChatKeyHistory> ChatKeyHistories { get; set; }
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
                      .WithMany(cm => cm.Messages)
                      .HasForeignKey(cm => cm.ChatId);
            });


            modelBuilder.Entity<ChatKeyHistory>(entity =>
            {
                entity.HasKey(ck => ck.Id);

                entity.HasOne<User>()
                      .WithMany()
                      .HasForeignKey(ck => ck.SenderUserId);

                entity.HasOne<User>()
                      .WithMany()
                      .HasForeignKey(ck => ck.ReceiverUserId);
            });

        }




    }
}
