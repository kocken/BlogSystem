namespace Domain
{
    public class PostEvaluation
    {
        public int PostID { get; set; }
        public int EvaluationID { get; set; }

        public Post Post { get; set; }
        public Evaluation Evaluation { get; set; }
    }
}
