using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Blog.Models;
using Data;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Domain;
using System.Linq;
using System;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Generic;

namespace Blog.Controllers
{
    public class HomeController : Controller
    {
        private readonly BlogContext _context;
        private readonly ILogger _logger;

        public HomeController(BlogContext context, ILogger<AccountController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            // Async used for faster load times, a future scalability feauture useful under heavy load.
            // Used instead of threading for faster perfomance.
            return View(await _context.Threads
                .Include(thread => thread.User)
                .Include(thread => thread.ThreadTags)
                    .ThenInclude(threadTags => threadTags.Tag)
                .Include(thread => thread.Comments)
                    .ThenInclude(comments => comments.User)
                .ToListAsync());
        }

        [HttpGet]
        [Route("Create-Thread")]
        public IActionResult CreateThread()
        {
            if (ViewBag.Username == null)
            {
                _logger.LogInformation("User tried to create thread without being logged in");
                TempData["Message"] = "You need to login to create threads";
                return RedirectToAction("Login", "Account");
            }
            if (ViewBag.RankLevel < 1)
            {
                _logger.LogInformation($"User \"{ViewBag.Username}\" tried to " +
                    "create thread without being high enough rank");
                TempData["Message"] = "You are not ranked high enough to create threads";
                return RedirectToAction("Index");
            }
            return View("Create-Thread", new ThreadModel { Tags = _context.Tags.ToList() });
        }

        [HttpPost]
        [Route("Create-Thread")]
        public IActionResult CreateThread(ThreadModel model)
        {
            if (ViewBag.Username == null)
            {
                _logger.LogInformation("User tried to create thread without being logged in");
                TempData["Message"] = "You need to login to create threads";
                return RedirectToAction("Login", "Account");
            }
            if (ViewBag.RankLevel < 1 && 
                !(model.Id >= 0 && model.UserId == ViewBag.UserId)) // if editing thread and not the author
            {
                if (model.Id == -1)
                {
                    _logger.LogInformation($"User \"{ViewBag.Username}\" tried to " +
                        "create thread without being high enough rank");
                    TempData["Message"] = "You are not ranked high enough to create threads";
                }
                else
                {
                    _logger.LogInformation($"User \"{ViewBag.Username}\" tried to edit the thread \"{model.Id}\" " +
                        "without being the author nor having high enough rank");
                    TempData["Message"] = "You are not the author of the thread, nor ranked high enough to edit the thread";
                }
                return RedirectToAction("Index");
            }
            if (ModelState.GetFieldValidationState("Title") != ModelValidationState.Valid ||
                ModelState.GetFieldValidationState("Message") != ModelValidationState.Valid)
            {
                _logger.LogInformation($"User \"{ViewBag.Username}\" tried to create thread with invalid model values");
                return View("Create-Thread", model);
            }
            if (ViewBag.User == null)
            {
                _logger.LogError($"Logged in user \"{ViewBag.Username}\" tried to make a thread, " +
                    "but the account was not found in the database");
                ViewBag.ErrorMessage = "Error: Your account was not found in the database. Try again.";
                return View("Create-Thread", model);
            }
            Thread thread = new Thread {
                Title = model.Title,
                Message = model.Message
            };
            if (model.Id == -1) // if creating new thread
            {
                thread.User = ViewBag.User;
                thread.CreationTime = DateTime.Now;
                _context.Add(thread);

                foreach (Tag tag in model.Tags)
                {
                    if (tag.Chosen)
                    {
                        _context.Add(new ThreadTag { ThreadId = thread.Id, TagId = tag.Id });
                    }
                }
            }
            else // if editing existing thread (model parameters passed from the EditThread action)
            {
                if (!_context.Threads.Any(t => t.Id == model.Id))
                {
                    _logger.LogError("Failed to obtain original thread " +
                        $"when user \"{ViewBag.Username}\" tried to edit thread \"{thread.Id}\"");
                    TempData["Message"] = "Error: Failed to obtain original thread. Try again.";
                    return RedirectToAction("Index");
                }
                if (model.UserId == -1 || model.CreationTime == null)
                {
                    _logger.LogError("Failed to obtain original thread " +
                        $"UserId \"{model.UserId}\" and/or CreationTime \"{model.CreationTime}\" " +
                        $"when user \"{ViewBag.Username}\" tried to edit thread \"{thread.Id}\"");
                    TempData["Message"] = "Error: Failed to obtain all thread values. Try again.";
                    return RedirectToAction("Index");
                }
                thread.Id = model.Id;
                thread.UserId = model.UserId;
                thread.CreationTime = model.CreationTime;
                _context.Update(thread);

                List<ThreadTag> threadTags = _context.ThreadTags
                    .Where(tt => tt.ThreadId == model.Id)
                    .Include(tt => tt.Tag)
                    .ToList();
                foreach (Tag tag in model.Tags)
                {
                    bool previouslyExisted = false;
                    foreach (ThreadTag tt in threadTags)
                    {
                        if (!tag.Chosen && tt.Tag.Name.Equals(tag.Name)) // if thread-tag was removed
                        {
                            _context.Remove(tt);
                        }
                        else if (tag.Chosen && tt.Tag.Name.Equals(tag.Name)) // if thread-tag existed and didn't get removed
                        {
                            previouslyExisted = true;
                        }
                    }
                    if (tag.Chosen && !previouslyExisted) // if new thread-tag
                    {
                        _context.Add(new ThreadTag { ThreadId = thread.Id, TagId = tag.Id });
                    }
                }
            }
            if (_context.SaveChanges() > 0)
            {
                _logger.LogInformation($"Thread \"{thread.Id}\" was " + 
                    (model.Id == -1 ? "created" : "edited") + $" by \"{ViewBag.Username}\"");
                TempData["Message"] = "Successfully " + (model.Id == -1 ? "created" : "edited") + " thread";
                return RedirectToAction("Index");
            }
            else
            {
                _logger.LogError("Saving changes returned <= 0 after " + (model.Id == -1 ? 
                    "adding" : "updating") + $" thread \"{thread.Id}\" " +
                    (model.Id == -1 ? "created" : "edited") + $" by \"{ViewBag.Username}\" to context");
                ViewBag.ErrorMessage = "Error: The database didn't register your " + 
                    (model.Id == -1 ? "thread" : "edit") + ". Try again.";
                return View("Create-Thread", model);
            }
        }

        [HttpGet]
        [Route("Edit-Thread")]
        public IActionResult EditThread(int id)
        {
            if (ViewBag.Username == null)
            {
                _logger.LogInformation("User tried to edit a thread without being logged in");
                TempData["Message"] = "You need to login to edit threads";
                return RedirectToAction("Login", "Account");
            }
            Thread thread = _context.Threads
                .Include(t => t.User)
                .SingleOrDefault(t => t.Id == id);
            if (thread == null)
            {
                _logger.LogError($"Thread \"{id}\" that was attempted to get edited " +
                    $"by \"{ViewBag.Username}\" was not found in the database");
                TempData["Message"] = "Error: The thread you tried to edit was not found in the database. Try again.";
                return RedirectToAction("Index");
            }
            if (ViewBag.RankLevel < 1 && !thread.User.Username.Equals(ViewBag.Username))
            {
                _logger.LogInformation($"User \"{ViewBag.Username}\" tried to edit the thread \"{id}\" " +
                    "without being the author nor having high enough rank");
                TempData["Message"] = "You are not the author nor ranked high enough to edit the thread";
                return RedirectToAction("Index");
            }
            List<Tag> tags = _context.Tags.ToList();
            List<ThreadTag> threadTags = _context.ThreadTags
                .Where(tt => tt.ThreadId == id)
                .Include(tt => tt.Tag)
                .ToList();
            foreach (ThreadTag tt in threadTags)
            {
                foreach (Tag t in tags)
                {
                    if (t.Name.Equals(tt.Tag.Name))
                    {
                        t.Chosen = true;
                        break;
                    }
                }
            }
            ThreadModel model = new ThreadModel
            {
                Id = id,
                UserId = thread.UserId,
                Title = thread.Title,
                Message = thread.Message,
                CreationTime = thread.CreationTime,
                Tags = tags
            };
            return View("Create-Thread", model);
        }

        [HttpGet]
        [Route("Post-Comment")]
        public IActionResult PostComment(int id)
        {
            if (ViewBag.Username == null)
            {
                _logger.LogInformation("User tried to post a comment without being logged in");
                TempData["Message"] = "You need to login to post comments";
                return RedirectToAction("Login", "Account");
            }
            if (ViewBag.RankLevel < 0)
            {
                _logger.LogInformation($"User \"{ViewBag.Username}\" tried to " +
                    "post a comment without being high enough rank");
                TempData["Message"] = "You are not ranked high enough to post comments";
                return RedirectToAction("Index");
            }
            return View("Post-Comment", new Comment { ThreadId = id });
        }

        [HttpPost]
        [Route("Post-Comment")]
        public IActionResult PostComment(Comment comment)
        {
            if (ViewBag.Username == null)
            {
                _logger.LogInformation("User tried to post a comment without being logged in");
                TempData["Message"] = "You need to login to post comments";
                return RedirectToAction("Login", "Account");
            }
            if (ViewBag.RankLevel < 0)
            {
                _logger.LogInformation($"User \"{ViewBag.Username}\" tried to " +
                    "post a comment without being high enough rank");
                TempData["Message"] = "You are not ranked high enough to post comments";
                return RedirectToAction("Index");
            }
            if (ModelState.GetFieldValidationState("Message") != ModelValidationState.Valid)
            {
                _logger.LogInformation($"User \"{ViewBag.Username}\" tried to post comment with invalid model value");
                return View("Post-Comment", comment);
            }
            if (ViewBag.User == null)
            {
                _logger.LogError($"Logged in user \"{ViewBag.Username}\" tried to make a thread, " +
                    "but the account was not found in the database");
                ViewBag.ErrorMessage = "Error: Your account was not found in the database. Try again.";
                return View("Post-Comment", comment);
            }
            if (!_context.Threads.Any(t => t.Id == comment.ThreadId))
            {
                _logger.LogError($"User \"{ViewBag.Username}\" tried to make a comment, " +
                    $"but the thread \"{comment.ThreadId}\" was not found in the database");
                ViewBag.ErrorMessage = "Error: The thread was not found in the database. Try again.";
                return View("Post-Comment", comment);
            }
            comment.User = ViewBag.User;
            comment.CreationTime = DateTime.Now;
            _context.Add(comment);
            if (_context.SaveChanges() > 0)
            {
                _logger.LogInformation($"Comment \"{comment.Id}\" was created by \"{ViewBag.Username}\"");
                TempData["Message"] = "Successfully created comment";
                return RedirectToAction("Index");
            }
            else
            {
                _logger.LogError("Saving changes returned <= 0 after adding " +
                    $"comment \"{comment.Id}\" made by \"{ViewBag.Username}\" to context");
                ViewBag.ErrorMessage = "Error: The database didn't register your comment. Try again.";
                return View("Post-Comment", comment);
            }
        }

        [HttpGet]
        [Route("Remove-Thread")]
        /// <summary>
        /// Deletes the thread and the connected posts (evaluations, comments & thread-tags) from the database. 
        /// <para>
        /// It's bad practice to delete data instead of choosing to not display it on the frontend with a property value, 
        /// but including it as executing deletion was a requirement for the assignment.
        /// </para>
        /// </summary>
        public IActionResult RemoveThread(int id)
        {
            if (ViewBag.Username == null)
            {
                _logger.LogInformation("User tried to remove a thread without being logged in");
                TempData["Message"] = "You need to login to remove threads";
                return RedirectToAction("Login", "Account");
            }
            Thread thread = _context.Threads.SingleOrDefault(t => t.Id == id);
            if (thread == null)
            {
                _logger.LogError($"User \"{ViewBag.Username}\" tried to remove thread " +
                    $"\"{id}\" that wasn't found in the database (already deleted?)");
                TempData["Message"] = "Error: The thread was not found in the database. Try again.";
                return RedirectToAction("Index");
            }
            if (ViewBag.RankLevel < 1 && thread.UserId != ViewBag.UserId)
            {
                _logger.LogInformation($"User \"{ViewBag.Username}\" tried to remove the thread \"{id}\" " +
                    "without being the author nor having high enough rank");
                TempData["Message"] = "You are not the author, nor ranked high enough to remove the thread";
                return RedirectToAction("Index");
            }
            _context.RemoveRange(_context.ThreadTags.Where(tt => tt.ThreadId == id));
            List<Comment> comments = _context.Comments.Where(c => c.ThreadId == id).IgnoreQueryFilters().ToList();
            foreach (Comment comment in comments)
            {
                _context.RemoveRange(_context.Evaluations.Where(e => e.CommentId == comment.Id));
                _context.Remove(comment);
            }
            _context.Remove(thread);
            if (_context.SaveChanges() > 0)
            {
                _logger.LogInformation
                    ($"User \"{ViewBag.Username}\" removed the thread \"{id}\" with its' connected posts");
                TempData["Message"] = "Successfully removed thread";
            }
            else
            {
                _logger.LogError($"Saving changes returned <= 0 after user \"{ViewBag.Username}\" " +
                    $"removed context thread \"{thread.Id}\" with its' connected posts");
                TempData["Message"] = "Error: The database did not register your deletion. Try again.";
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Route("Admin-Panel")]
        public IActionResult AdminPanel()
        {
            if (ViewBag.Username == null)
            {
                _logger.LogInformation("User tried to visit the admin-panel without being logged in");
                TempData["Message"] = "You need to login to see this page";
                return RedirectToAction("Login", "Account");
            }
            if (ViewBag.RankLevel < 2)
            {
                _logger.LogInformation($"User \"{ViewBag.Username}\" tried to " +
                    "visit the admin-panel without being high enough rank");
                TempData["Message"] = "You are not ranked high enough to see this page";
                return RedirectToAction("Index");
            }
            return View("Admin-Panel");
        }

        [HttpGet]
        public async Task<IActionResult> Moderation()
        {
            if (ViewBag.Username == null)
            {
                _logger.LogInformation("User tried to visit the moderation page without being logged in");
                TempData["Message"] = "You need to login to see this page";
                return RedirectToAction("Login", "Account");
            }
            if (ViewBag.RankLevel < 1)
            {
                _logger.LogInformation($"User \"{ViewBag.Username}\" tried to " +
                    "visit the moderation page without being high enough rank");
                TempData["Message"] = "You are not ranked high enough to see this page";
                return RedirectToAction("Index");
            }
            return View(await _context.Comments
                .IgnoreQueryFilters()
                .Include(c => c.User)
                .Include(c => c.Thread)
                .Include(c => c.Evaluations)
                    .ThenInclude(e => e.EvaluationValue)
                .Include(c => c.User)
                    .ThenInclude(u => u.Comments)
                        .ThenInclude(c => c.Evaluations)
                            .ThenInclude(e => e.EvaluationValue)
                .ToListAsync());
        }

        [HttpGet]
        public IActionResult About()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
