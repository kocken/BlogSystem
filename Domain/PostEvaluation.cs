using System.ComponentModel.DataAnnotations;

namespace Domain
{
    public class PostEvaluation
    {
        [Key, Required]
        public int PostId { get; set; }
        [Key, Required]
        public int EvaluationId { get; set; }

        [Required]
        public Post Post { get; set; }
        [Required]
        public Evaluation Evaluation { get; set; }
    }
}
