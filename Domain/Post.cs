using System;

namespace Domain
{
    public class Post
    {
        public int ID { get; set; }
        public User Author { get; set; }
        public string Message { get; set; }
        public DateTime CreationTime { get; set; }
    }
}
