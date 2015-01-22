using System;

namespace Assets.Sources.DatabaseServer.Models
{
    public class User
    {
        public const int MAX_USERNAME_LEN = 15;
        public const int MIN_USERNAME_LEN = 2;

        public Int64 UserId { get; set; }
        public string Username { get; set; }
        public int Ranking { get; set; }
    }
}
