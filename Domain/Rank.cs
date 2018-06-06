using System.ComponentModel.DataAnnotations;

namespace Domain
{
    public class Rank
    {
        [Required]
        public int Id { get; set; }

        [Required, StringLength(20, MinimumLength = 1)]
        public string Name { get; set; }
    }
}
