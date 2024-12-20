﻿using TestingApp.Core.Models.Identity.Enums;

namespace TestingApp.Core.Models.Identity
{
    public class User
    {
        public Guid ID { get; set; }
        public required string Name { get; set; }
        public required string Login { get; set; }
        public required string Password { get; set; }
        public RoleType RoleType { get; set; }
    }
}
