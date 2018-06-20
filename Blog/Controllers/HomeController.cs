using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Blog.Models;
using Data;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using Domain;

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
            List<Thread> threads = _context.Threads
                .Include(thread => thread.User)
                .Include(thread => thread.Comments)
                    .ThenInclude(comments => comments.Evaluations)
                        .ThenInclude(evaluations => evaluations.EvaluationValue)
                .ToList();
            return View(await _context.Threads
                .Include(thread => thread.User)
                .Include(thread => thread.Comments)
                    .ThenInclude(comments => comments.Evaluations)
                        .ThenInclude(evaluations => evaluations.EvaluationValue)
                .ToListAsync());
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
