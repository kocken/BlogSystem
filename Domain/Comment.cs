using System;
using System.ComponentModel.DataAnnotations;

namespace Domain
{
    public class Comment
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public User User { get; set; }
        [Required]
        public Thread Thread { get; set; }
        [Required, StringLength(2000, MinimumLength = 1)]
        public string Message { get; set; }
        [Required]
        public DateTime CreationTime { get; set; }
    }
}
