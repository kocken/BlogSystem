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

        public async Task<IActionResult> Index()
        {
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
            CreateThreadModel model = new CreateThreadModel();
            model.Tags = _context.Tags.ToList();
            return View("Create-Thread", model);
        }

        [HttpPost]
        [Route("Create-Thread")]
        public IActionResult CreateThread(CreateThreadModel model)
        {
            if (ModelState.GetFieldValidationState("Title") == ModelValidationState.Valid &&
                ModelState.GetFieldValidationState("Message") == ModelValidationState.Valid)
            {
                if (Util.GetUsername(HttpContext.Session, out string username))
                {
                    Thread thread = new Thread { Title = model.Title, Message = model.Message };
                    User user = _context.Users.Single(u => u.Username.Equals(username));
                    if (user != null)
                    {
                        thread.User = user;
                        thread.CreationTime = DateTime.Now;
                        _context.Update(thread);
                        foreach (Tag tag in model.Tags)
                        {
                            if (tag.Chosen)
                            {
                                ThreadTag threadTag = new ThreadTag();
                                threadTag.Thread = thread;
                                threadTag.Tag = tag;
                                _context.Update(threadTag);
                            }
                        }
                        if (_context.SaveChanges() > 0)
                        {
                            _logger.LogInformation($"Thread \"{thread.Id}\" was created by \"{user.Username}\"");
                            TempData["Message"] = "Successfully created thread";
                            return RedirectToAction("Index");
                        }
                        else
                        {
                            _logger.LogError
                                ($"Saving changes returned <= 0 after updating context with thread \"{thread.Id}\" made by \"{user.Username}\"");
                            ViewBag.ErrorMessage = "Error: The database didn't register your thread. Try again.";
                        }
                    }
                    else
                    {
                        _logger.LogError
                            ($"Logged in user \"{user.Username}\" tried to make a thread, " +
                            $"but the account was not found in the database");
                        ViewBag.ErrorMessage = "Error: Your account was not found in the database. Try again.";
                    }
                }
                else
                {
                    _logger.LogInformation
                        ("User tried to create thread without being logged in");
                    ViewBag.ErrorMessage = "Error: You need to login to create a thread.";
                }
            }
            return View("Create-Thread", model);
        }

        [Route("Create-Comment")]
        public IActionResult CreateComment(int id)
        {
            CreateThreadModel model = new CreateThreadModel();
            model.Tags = _context.Tags.ToList();
            return View("Create-Comment", model);
        }

        [Route("Edit-Thread")]
        public IActionResult EditThread(int id)
        {
            CreateThreadModel model = new CreateThreadModel();
            model.Tags = _context.Tags.ToList();
            return View("Edit-Thread", model);
        }

        [Route("Remove-Thread")]
        public IActionResult RemoveThread(int id)
        {
            Thread thread = _context.Threads.Single(t => t.Id == id);
            Util.GetUsername(HttpContext.Session, out string username);
            if (thread != null)
            {
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
                        ($"User \"{username}\" removed the thread \"{id}\" with its' connected posts");
                    TempData["Message"] = "Successfully removed thread";
                }
                else
                {
                    _logger.LogError
                        ($"Saving changes returned <= 0 after user \"{username}\" " +
                        $"removed context thread \"{thread.Id}\" with its' connected posts");
                    TempData["Message"] = "Error: The database didn't register your deletion. Try again.";
                }
            }
            else
            {
                _logger.LogInformation
                    ($"User \"{username}\" tried to remove thread \"{id}\" that wasn't found in the database (already deleted?)");
                TempData["Message"] = "Error: The thread was not found in the database";
            }
            return RedirectToAction("Index");
        }

        [Route("Admin-Panel")]
        public async Task<IActionResult> AdminPanel()
        {
            return View("Admin-Panel", await _context.Threads.Include(_ => _.User).ToListAsync());
        }

        public async Task<IActionResult> Moderation()
        {
            return View(await _context.Threads.Include(_ => _.User).ToListAsync());
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
