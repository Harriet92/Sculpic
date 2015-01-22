using System;

namespace Assets.Sources.DatabaseClient.Models
{
    public class User
    {
        public const int MAX_USERNAME_LEN = 15;
        public const int MIN_USERNAME_LEN = 2;

        public Int64 UserId { get; set; }
        public string Username { get; set; }
        public DateTime LastLoginAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public int Ranking { get; set; }

    }
}
