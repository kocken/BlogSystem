using System;
using System.ComponentModel.DataAnnotations;

namespace Domain
{
    public class Post
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public User Author { get; set; }
        [Required, MaxLength(2000)]
        public string Message { get; set; }
        [Required]
        public DateTime CreationTime { get; set; }
    }
}
