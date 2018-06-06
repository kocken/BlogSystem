using System;

namespace Domain
{
    public enum Rank
    {
        Member, Moderator, Administrator
    }

    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public Rank Rank { get; set; }
        public DateTime JoinTime { get; set; }
    }
}
