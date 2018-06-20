using Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Blog.Models
{
    public class CreateThreadModel
    {
        [Required, StringLength(80, MinimumLength = 1)]
        public string Title { get; set; }

        [Required, StringLength(2000, MinimumLength = 1)]
        public string Message { get; set; }

        public List<Tag> Tags { get; set; }
    }
}