using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatMessages.Domain.Entities
{
    public class Chat
    {
        public int Id { get; set; }

        public int SenderUserId { get; set; }
        public int ReceiverUserId { get; set; }

        public string SenderKeyPath { get; set; } = string.Empty;
        public DateTime SenderKeyExpirationAt { get; set; }

        public string ReceiverKeyPath { get; set; } = string.Empty;
        public DateTime ReceiverKeyExpirationAt { get; set; }

        public string SenderPublicKey { get; set; } = string.Empty;
        public string ReceiverPublicKey { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }

        public ICollection<ChatMessage> Messages { get; set; } = new List<ChatMessage>();
        public ICollection<ChatKeyHistory> KeyHistories { get; set; } = new List<ChatKeyHistory>();
    }
}
