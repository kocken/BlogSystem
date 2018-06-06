using System.ComponentModel.DataAnnotations;

namespace Domain
{
    public class Comment : Post
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public Thread Thread { get; set; }
    }
}
