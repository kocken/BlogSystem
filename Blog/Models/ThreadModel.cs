using Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Blog.Models
{
    public class ThreadModel
    {
        public ThreadModel()
        {
            Id = -1;
            UserId = -1;
        }

        public int Id { get; set; }

        public int UserId { get; set; }

        [Required, StringLength(80, MinimumLength = 1)]
        public string Title { get; set; }

        [Required, StringLength(2000, MinimumLength = 1)]
        public string Message { get; set; }

        public DateTime CreationTime { get; set; }

        public List<Tag> Tags { get; set; }
    }
}