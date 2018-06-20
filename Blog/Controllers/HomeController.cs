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
using System.Text;

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
            Thread thread = new Thread();
            thread.Title = model.Title;
            thread.Message = model.Message;
            thread.CreationTime = DateTime.Now;
            if (HttpContext.Session.TryGetValue("Username", out byte[] value))
            {
                string username = Encoding.UTF8.GetString(value);
                User user = _context.Users.Single(u => u.Username.Equals(username));
                if (user != null)
                {
                    thread.User = user;
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
                    _context.SaveChanges();
                }
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
