﻿namespace TestingApp.Database.Models
{
    public class User
    {
        public Guid ID { get; set; }
        public required string Name { get; set; }
        public required string Login { get; set; }
        public required string Password { get; set; }
    }
}