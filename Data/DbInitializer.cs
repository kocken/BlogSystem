using Domain;
using System;
using System.Linq;

namespace Data
{
    public static class DbInitializer
    {
        private static bool ReInitialize = false; // NOTE: when true the DB will be reset with the entries below

        public static void Initialize(BlogContext context)
        {
            context.Database.EnsureCreated();

            if (ReInitialize) // removes all entries from database
            {
                context.ThreadTags.RemoveRange(context.ThreadTags.Where(t => t.ThreadId == t.ThreadId));
                context.Tags.RemoveRange(context.Tags.Where(t => t.Id == t.Id));
                context.Evaluations.RemoveRange(context.Evaluations.Where(e => e.Id == e.Id));
                context.EvaluationValues.RemoveRange(context.EvaluationValues.Where(e => e.Id == e.Id));
                context.Comments.RemoveRange(context.Comments.Where(c => c.Id == c.Id));
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
                new Rank{ Name = "Member" },
                new Rank{ Name = "Moderator" },
                new Rank{ Name = "Administrator" }
            };
            foreach (Rank r in ranks)
            {
                context.Ranks.Add(r);
            }
            context.SaveChanges();


            User[] users = new User[]
            {
                new User{
                    Username = "Mikael",
                    Password = "pass123",
                    Rank = Array.Find(ranks, r => r.Name.Equals("Administrator")),
                    JoinTime = DateTime.Now
                },
                new User{
                    Username = "Mikael2",
                    Password = "pass123",
                    Rank = Array.Find(ranks, r => r.Name.Equals("Moderator")),
                    JoinTime = DateTime.Now
                },
                new User{
                    Username = "Billy",
                    Password = "qwerty",
                    Rank = Array.Find(ranks, r => r.Name.Equals("Member")),
                    JoinTime = DateTime.Now
                }
            };
            foreach (User u in users)
            {
                context.Users.Add(u);
            }
            context.SaveChanges();


            Thread[] threads = new Thread[]
            {
                new Thread{
                    User = Array.Find(users, u => u.Username.Equals("Mikael")),
                    Title = "First thread",
                    Message = "This is the first thread made!",
                    CreationTime = DateTime.Now
                }
            };
            foreach (Thread t in threads)
            {
                context.Threads.Add(t);
            }
            context.SaveChanges();


            Comment[] comments = new Comment[]
            {
                new Comment{
                    User = Array.Find(users, u => u.Username.Equals("Billy")),
                    Thread = threads[0],
                    Message = "Cool!",
                    CreationTime = DateTime.Now
                },
                new Comment{
                    User = Array.Find(users, u => u.Username.Equals("Billy")),
                    Thread = threads[0],
                    Message = "Can I get mod?",
                    CreationTime = DateTime.Now
                }
            };
            foreach (Comment c in comments)
            {
                context.Comments.Add(c);
            }
            context.SaveChanges();


            EvaluationValue[] evaluationValues = new EvaluationValue[]
            {
                new EvaluationValue{ Name = "Approved" },
                new EvaluationValue{ Name = "Disapproved" }
            };
            foreach (EvaluationValue e in evaluationValues)
            {
                context.EvaluationValues.Add(e);
            }
            context.SaveChanges();


            Evaluation[] evaluations = new Evaluation[]
            {
                new Evaluation{
                    Comment = comments[0],
                    EvaluationValue = Array.Find(evaluationValues, e => e.Name.Equals("Approved")),
                    EvaluatedBy = Array.Find(users, u => u.Username.Equals("Mikael")),
                    EvaluationTime = DateTime.Now
                },
                new Evaluation{
                    Comment = comments[0],
                    EvaluationValue = Array.Find(evaluationValues, e => e.Name.Equals("Approved")),
                    EvaluatedBy = Array.Find(users, u => u.Username.Equals("Mikael2")),
                    EvaluationTime = DateTime.Now
                }
            };
            foreach (Evaluation e in evaluations)
            {
                context.Evaluations.Add(e);
            }
            context.SaveChanges();


            Tag[] tags = new Tag[]
            {
                new Tag{ Name = "Comedy" }, 
                new Tag{ Name = "Information" }, 
                new Tag{ Name = "Political" },
                new Tag{ Name = "Sponsored" },
                new Tag{ Name = "Discussion" },
                new Tag{ Name = "Announcement" }
            };
            foreach (Tag t in tags)
            {
                context.Tags.Add(t);
            }
            context.SaveChanges();


            ThreadTag[] threadTags = new ThreadTag[]
            {
                new ThreadTag{
                    Thread = threads[0],
                    Tag = Array.Find(tags, t => t.Name.Equals("Discussion"))
                },
                new ThreadTag{
                    Thread = threads[0],
                    Tag = Array.Find(tags, t => t.Name.Equals("Announcement"))
                }
            };
            foreach (ThreadTag t in threadTags)
            {
                context.ThreadTags.Add(t);
            }
            context.SaveChanges();
        }
    }
}
