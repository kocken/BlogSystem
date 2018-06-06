using System;
using System.ComponentModel.DataAnnotations;

namespace Domain
{
    public class User
    {
        [Required]
        public int Id { get; set; }
        [Required, MaxLength(20)]
        public string Username { get; set; }
        [Required, MaxLength(20)]
        public string Password { get; set; }
        [Required]
        public Rank Rank { get; set; }
        [Required]
        public DateTime JoinTime { get; set; }
    }
}
