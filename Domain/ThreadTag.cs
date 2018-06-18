using System.ComponentModel.DataAnnotations;

namespace Domain
{
    public class ThreadTag
    {
        [Key, Required]
        public int ThreadId { get; set; }

        [Required]
        public Thread Thread { get; set; }

        [Key, Required]
        public int TagId { get; set; }

        [Required]
        public Tag Tag { get; set; }
    }
}
