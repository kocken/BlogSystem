using System.ComponentModel.DataAnnotations;

namespace Domain
{
    public class EvaluationValue
    {
        [Required]
        public int Id { get; set; }
        [Required, MaxLength(20)]
        public string Name { get; set; }
    }
}
