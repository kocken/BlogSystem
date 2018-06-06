using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain
{
    public class Thread
    {
        public Thread()
        {
            Comments = new List<Comment>();
        }

        [Required]
        public int Id { get; set; }
        [Required]
        public User User { get; set; }
        [Required, StringLength(20, MinimumLength = 1)]
        public string Title { get; set; }
        [Required, StringLength(2000, MinimumLength = 1)]
        public string Message { get; set; }
        [Required]
        public DateTime CreationTime { get; set; }

        public List<Comment> Comments { get; set; }
    }
}
