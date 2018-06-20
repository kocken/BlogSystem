using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain
{
    public enum Tags
    {
        Comedy, Information, Political, Sponsored, Discussion, Announcement
    }

    public class Tag
    {
        [Key, Required]
        public int Id { get; set; }

        [Required, StringLength(20, MinimumLength = 1)]
        public string Name { get; set; }

        [NotMapped]
        public bool Chosen { get; set; }
    }
}
