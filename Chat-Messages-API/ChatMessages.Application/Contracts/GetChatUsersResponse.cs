using ChatMessages.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatMessages.Application.Contracts
{
    public class GetChatUsersResponse
    {
        public int OtherUserId { get; set; }
        public string? OtherUserName { get; set; }
        public string? StatusChat { get; set; }
        public bool Accepted { get; set; }
        public int ChatId { get; set; }
    }
}
