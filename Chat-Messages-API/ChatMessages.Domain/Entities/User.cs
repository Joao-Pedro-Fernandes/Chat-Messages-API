﻿namespace ChatMessages.Domain.Entities
{
    public sealed class User
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Password { get; set; }
        public DateTime? LastAcessAt { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}
