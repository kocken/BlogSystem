using System;

namespace Domain
{
    public enum Value
    {
        Approved, Disapproved
    }

    public class Evaluation
    {
        public int Id { get; set; }
        public Post Post { get; set; }
        public Value? Value { get; set; }
        public User EvaluatedBy { get; set; }
        public User EvaluatedOn { get; set; }
        public DateTime EvaluationTime { get; set; }
    }
}
