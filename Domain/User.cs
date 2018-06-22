using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain
{
    public class User
    {
        public User()
        {
            Comments = new List<Comment>();
        }

        [Key, Required]
        public int Id { get; set; }

        [Required(ErrorMessage = "Username is required."), 
            StringLength(20, MinimumLength = 1, ErrorMessage = "Username needs to be between 1 and 20 characters."), 
            RegularExpression("^[a-zA-Z0-9]+$", ErrorMessage = "Username can only contain letters and numbers."),
            Remote(action: "IsUsernameAvailable", controller: "Account")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is required."), 
            StringLength(20, MinimumLength = 5, ErrorMessage = "Password needs to be between 5 and 20 characters."), 
            RegularExpression("^[a-zA-Z0-9]+$", ErrorMessage = "Password can only contain letters and numbers.")]
        public string Password { get; set; }

        [Required]
        public Rank Rank { get; set; }

        [Required]
        public DateTime JoinTime { get; set; }

        public List<Comment> Comments { get; set; }
    }
}
