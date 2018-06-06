using System;
using System.ComponentModel.DataAnnotations;

namespace Domain
{
    public class User
    {
        [Required]
        public int Id { get; set; }
        [Required, StringLength(20, MinimumLength = 1)]
        public string Username { get; set; }
        [Required, StringLength(20, MinimumLength = 5)]
        public string Password { get; set; }
        [Required]
        public Rank Rank { get; set; }
        [Required]
        public DateTime JoinTime { get; set; }
    }
}
