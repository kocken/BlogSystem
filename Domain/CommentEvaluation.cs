using System.ComponentModel.DataAnnotations;

namespace Domain
{
    public class CommentEvaluation
    {
        [Key, Required]
        public int CommentId { get; set; }
        [Key, Required]
        public int EvaluationId { get; set; }

        [Required]
        public Comment Comment { get; set; }
        [Required]
        public Evaluation Evaluation { get; set; }
    }
}
