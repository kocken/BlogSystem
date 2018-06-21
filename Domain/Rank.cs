using System.ComponentModel.DataAnnotations;

namespace Domain
{
    public enum Ranks
    {
        Member = 0, Moderator = 1, Administrator = 2
    }

    public class Rank
    {
        [Key, Required]
        public int Id { get; set; }

        [Required, StringLength(20, MinimumLength = 1)]
        public string Name { get; set; }
    }
}
