﻿namespace EventBus.Entities.Identity
{
    public class IdentityModelCreateUser
    {
        public string? Nickname { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; } = "";
        public string? PasswordHash { get; set; }
        public string? PasswordSalt { get; set; }
        public Guid RoleId { get; set; }
    }
}
