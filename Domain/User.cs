using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public enum Rank
    {
        Member, Moderator, Administrator
    }

    public class User
    {
        public User()
        {
            Threads = new List<Thread>();
            Comments = new List<Post>();
            Evaluations = new List<Evaluation>();
            EvaluationsCreated = new List<Evaluation>();
        }

        public int ID { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public Rank Rank { get; set; }
        public DateTime JoinTime { get; set; }
        public List<Thread> Threads { get; set; }
        public List<Post> Comments { get; set; }
        public List<Evaluation> Evaluations { get; set; } // evulations ON the user
        public List<Evaluation> EvaluationsCreated { get; set; } // evaluations MADE BY the user, only applies to staff
    }
}
