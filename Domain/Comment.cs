using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain
{
    public class Comment : Post
    {
        public Comment()
        {
            Evaluations = new List<Evaluation>();
        }

        [Key, Required]
        public int Id { get; set; }

        [Required]
        public int ThreadId { get; set; }

        [Required]
        public Thread Thread { get; set; }

        public List<Evaluation> Evaluations { get; set; }
    }
}
