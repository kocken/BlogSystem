using System;
using System.ComponentModel.DataAnnotations;

namespace Domain
{
    public class Post
    {
        [Required]
        public int UserId { get; set; }

        [Required]
        public User User { get; set; }

        [Required, StringLength(2000, MinimumLength = 1)]
        public string Message { get; set; }

        [Required]
        public DateTime CreationTime { get; set; }
    }
}
