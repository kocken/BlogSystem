namespace Domain
{
    public class PostEvaluation
    {
        public int PostId { get; set; }
        public int EvaluationId { get; set; }

        public Post Post { get; set; }
        public Evaluation Evaluation { get; set; }
    }
}
