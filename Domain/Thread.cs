﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain
{
    public class Thread : Post
    {
        public Thread()
        {
            Comments = new List<Post>();
        }

        [Required, MaxLength(20)]
        public string Title { get; set; }
        public List<Post> Comments { get; set; }
    }
}
