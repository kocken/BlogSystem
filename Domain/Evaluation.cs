using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Evaluation
    {
        public int Id { get; set; }
        public Post Post { get; set; }
        public EvaluationValue Value { get; set; }
        public User EvaluatedBy { get; set; }
        public DateTime EvaluationTime { get; set; }
    }
}
