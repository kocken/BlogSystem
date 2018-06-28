using Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace Data
{
    public static class DbInitializer
    {
        private static readonly bool ReInitialize = false; // NOTE: when true the DB will be reset with the entries below

        public static void Initialize(BlogContext context)
        {
            context.Database.EnsureCreated();

            if (ReInitialize) // removes all entries from database
            {
                context.ThreadTags.RemoveRange(context.ThreadTags.Where(t => t.ThreadId == t.ThreadId));
                context.Tags.RemoveRange(context.Tags.Where(t => t.Id == t.Id));
                context.Evaluations.RemoveRange(context.Evaluations.Where(e => e.Id == e.Id));
                context.EvaluationValues.RemoveRange(context.EvaluationValues.Where(e => e.Id == e.Id));
                context.Comments.RemoveRange(context.Comments.Where(c => c.Id == c.Id).IgnoreQueryFilters());
                context.Threads.RemoveRange(context.Threads.Where(t => t.Id == t.Id));
                context.Users.RemoveRange(context.Users.Where(u => u.Id == u.Id));
                context.Ranks.RemoveRange(context.Ranks.Where(r => r.Id == r.Id));

                context.SaveChanges();
            }

            if (context.Ranks.Any()) // true if the code below have been run before and ReInitialize = false
            {
                return; // DB has been seeded, end method
            }


            Rank[] ranks = new Rank[]
            {
                new Rank{ Name = Ranks.Member.ToString() },
                new Rank{ Name = Ranks.Moderator.ToString() },
                new Rank{ Name = Ranks.Administrator.ToString() }
            };
            foreach (Rank r in ranks.Reverse())
            {
                context.Ranks.Add(r);
            }
            context.SaveChanges();


            User[] users = new User[]
            {
                new User{
                    Username = "Admin",
                    Password = Security.ROT13EncryptMessage("pass123"),
                    Rank = Array.Find(ranks, r => r.Name.Equals(Ranks.Administrator.ToString())),
                    JoinTime = DateTime.Now
                },
                new User{
                    Username = "Mikael",
                    Password = Security.ROT13EncryptMessage("pass123"),
                    Rank = Array.Find(ranks, r => r.Name.Equals(Ranks.Moderator.ToString())),
                    JoinTime = DateTime.Now + TimeSpan.FromSeconds(5)
                },
                new User{
                    Username = "Billy",
                    Password = Security.ROT13EncryptMessage("qwerty"),
                    Rank = Array.Find(ranks, r => r.Name.Equals(Ranks.Member.ToString())),
                    JoinTime = DateTime.Now + TimeSpan.FromSeconds(10)
                }
            };
            foreach (User u in users.Reverse())
            {
                context.Users.Add(u);
            }
            context.SaveChanges();


            Thread[] threads = new Thread[]
            {
                new Thread{
                    User = Array.Find(users, u => u.Username.Equals("Admin")),
                    Title = "#1",
                    Message = "This is the first thread made!",
                    CreationTime = DateTime.Now
                },
                new Thread{
                    User = Array.Find(users, u => u.Username.Equals("Mikael")),
                    Title = "Second thread test",
                    Message = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, " +
                    "sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. " +
                    "Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris " +
                    "nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in " +
                    "reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla" +
                    " pariatur. Excepteur sint occaecat cupidatat non proident, sunt in " +
                    "culpa qui officia deserunt mollit anim id est laborum.",
                    CreationTime = DateTime.Now + TimeSpan.FromSeconds(30)
                }
            };
            foreach (Thread t in threads.Reverse())
            {
                context.Threads.Add(t);
            }
            context.SaveChanges();


            Tag[] tags = new Tag[]
            {
                new Tag{ Name = Tags.Comedy.ToString() },
                new Tag{ Name = Tags.Information.ToString() },
                new Tag{ Name = Tags.Political.ToString() },
                new Tag{ Name = Tags.Sponsored.ToString() },
                new Tag{ Name = Tags.Discussion.ToString() },
                new Tag{ Name = Tags.Announcement.ToString() }
            };
            foreach (Tag t in tags.Reverse())
            {
                context.Tags.Add(t);
            }
            context.SaveChanges();


            ThreadTag[] threadTags = new ThreadTag[]
            {
                new ThreadTag{
                    Thread = threads[0],
                    Tag = Array.Find(tags, t => t.Name.Equals(Tags.Discussion.ToString()))
                },
                new ThreadTag{
                    Thread = threads[0],
                    Tag = Array.Find(tags, t => t.Name.Equals(Tags.Announcement.ToString()))
                }
            };
            foreach (ThreadTag t in threadTags.Reverse())
            {
                context.ThreadTags.Add(t);
            }
            context.SaveChanges();


            Comment[] comments = new Comment[]
            {
                new Comment{
                    User = Array.Find(users, u => u.Username.Equals("Billy")),
                    Thread = threads[0],
                    Message = "Cool!",
                    CreationTime = DateTime.Now + TimeSpan.FromSeconds(20)
                },
                new Comment{
                    User = Array.Find(users, u => u.Username.Equals("Billy")),
                    Thread = threads[0],
                    Message = "Disapprove this comment",
                    CreationTime = DateTime.Now + TimeSpan.FromSeconds(40)
                },
                new Comment{
                    User = Array.Find(users, u => u.Username.Equals("Mikael")),
                    Thread = threads[1],
                    Message = "Sed ut perspiciatis unde omnis iste natus error " +
                    "sit voluptatem accusantium doloremque laudantium, totam rem" +
                    " aperiam, eaque ipsa quae ab illo inventore veritatis et quasi" +
                    " architecto beatae vitae dicta sunt explicabo. Nemo enim ipsam" +
                    " voluptatem quia voluptas sit aspernatur aut odit aut fugit, " +
                    "sed quia consequuntur magni dolores eos qui ratione voluptatem" +
                    " sequi nesciunt. Neque porro quisquam est, qui dolorem ipsum " +
                    "quia dolor sit amet, consectetur, adipisci velit, sed quia non" +
                    " numquam eius modi tempora incidunt ut labore et dolore magnam" +
                    " aliquam quaerat voluptatem. Ut enim ad minima veniam, quis " +
                    "nostrum exercitationem ullam corporis suscipit laboriosam, " +
                    "nisi ut aliquid ex ea commodi consequatur? Quis autem vel eum" +
                    " iure reprehenderit qui in ea voluptate velit esse quam nihil " +
                    "molestiae consequatur, vel illum qui dolorem eum fugiat quo " +
                    "voluptas nulla pariatur?",
                    CreationTime = DateTime.Now + TimeSpan.FromSeconds(60)
                },
                new Comment{
                    User = Array.Find(users, u => u.Username.Equals("Billy")),
                    Thread = threads[0],
                    Message = "Can I get mod?",
                    CreationTime = DateTime.Now + TimeSpan.FromSeconds(80)
                },
                new Comment{
                    User = Array.Find(users, u => u.Username.Equals("Billy")),
                    Thread = threads[0],
                    Message = "Hello???",
                    CreationTime = DateTime.Now + TimeSpan.FromSeconds(600)
                }
            };
            foreach (Comment c in comments.Reverse())
            {
                context.Comments.Add(c);
            }
            context.SaveChanges();


            EvaluationValue[] evaluationValues = new EvaluationValue[]
            {
                new EvaluationValue{ Name = EvaluationValues.Approved.ToString() },
                new EvaluationValue{ Name = EvaluationValues.Disapproved.ToString() }
            };
            foreach (EvaluationValue e in evaluationValues.Reverse())
            {
                context.EvaluationValues.Add(e);
            }
            context.SaveChanges();


            Evaluation[] evaluations = new Evaluation[]
            {
                new Evaluation{
                    Comment = comments[0],
                    EvaluationValue = Array.Find(evaluationValues, e => e.Name.Equals(EvaluationValues.Approved.ToString())),
                    EvaluatedBy = Array.Find(users, u => u.Username.Equals("Admin")),
                    EvaluationTime = DateTime.Now + TimeSpan.FromSeconds(40)
                },
                new Evaluation{
                    Comment = comments[0],
                    EvaluationValue = Array.Find(evaluationValues, e => e.Name.Equals(EvaluationValues.Approved.ToString())),
                    EvaluatedBy = Array.Find(users, u => u.Username.Equals("Mikael")),
                    EvaluationTime = DateTime.Now + TimeSpan.FromSeconds(60)
                },
                new Evaluation{
                    Comment = comments[1],
                    EvaluationValue = Array.Find(evaluationValues, e => e.Name.Equals(EvaluationValues.Disapproved.ToString())),
                    EvaluatedBy = Array.Find(users, u => u.Username.Equals("Mikael")),
                    EvaluationTime = DateTime.Now + TimeSpan.FromSeconds(80)
                },
                new Evaluation{
                    Comment = comments[2],
                    EvaluationValue = Array.Find(evaluationValues, e => e.Name.Equals(EvaluationValues.Approved.ToString())),
                    EvaluatedBy = Array.Find(users, u => u.Username.Equals("Mikael")),
                    EvaluationTime = DateTime.Now + TimeSpan.FromSeconds(100)
                }
            };
            foreach (Evaluation e in evaluations.Reverse())
            {
                context.Evaluations.Add(e);
            }
            context.SaveChanges();
        }
    }
}
