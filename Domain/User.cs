using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public enum Rank
    {
            Member, Mod, Admin
    }

    public class User
    {
        public User()
        {
            Threads = new List<Thread>();
            Posts = new List<Post>();
            Evaluations = new List<Evaluation>();
        }

        public int ID { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public Rank? Rank { get; set; }
        public DateTime JoinTime { get; set; }
        public List<Thread> Threads { get; set; }
        public List<Post> Posts { get; set; }
        public List<Evaluation> Evaluations { get; set; }
    }
}
