using System;
using System.ComponentModel.DataAnnotations;

namespace Domain
{
    public class Evaluation
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public Post Post { get; set; }
        [Required]
        public EvaluationValue EvaluationValue { get; set; }
        [Required]
        public User EvaluatedBy { get; set; }
        [Required]
        public User EvaluatedOn { get; set; }
        [Required]
        public DateTime EvaluationTime { get; set; }
    }
}
