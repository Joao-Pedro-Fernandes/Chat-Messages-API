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
        public DateTime CreatedAt { get; set; }
        public User SenderUser { get; set; }
        public User ReceiverUser { get; set; }

    }
}
