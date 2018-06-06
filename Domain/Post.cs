using System;

namespace Domain
{
    public class Post
    {
        public int Id { get; set; }
        public User Author { get; set; }
        public string Message { get; set; }
        public DateTime CreationTime { get; set; }
    }
}
