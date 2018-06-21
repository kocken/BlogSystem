using Data;
using Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

namespace Blog
{
    public class GlobalFilter : IActionFilter
    {
        private readonly BlogContext _context;
        private readonly ILogger _logger;

        public GlobalFilter(BlogContext context, ILogger<GlobalFilter> logger)
        {
            _context = context;
            _logger = logger;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            Controller controller = context.Controller as Controller;

            string path = context.HttpContext.Request.Path.ToString().ToLower();
            if (path.EndsWith("/login") || path.EndsWith("/register")) // floating label pages
            {
                controller.ViewBag.ShowFloatingLabels = FloatingLabelsCompatibleBrowser(context.HttpContext);
            }

            string username = context.HttpContext.Session.GetString("Username");
            controller.ViewBag.Username = username;
            controller.ViewBag.RankLevel = GetRankLevel(context, controller, username);
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            //code here runs after the action method executes
        }

        private int GetRankLevel(ActionExecutingContext context, Controller controller, string username)
        {
            if (username == null) return -1;
            User user = _context.Users
                .Include(u => u.Rank)
                .SingleOrDefault(u => u.Username.Equals(username));
            if (user == null)
            {
                context.HttpContext.Session.Remove("Username");
                _logger.LogError($"Logged in user \"{username}\" was not found in the database");
                controller.TempData["Message"] = "Error: The account you are logged into was not found in the database. " +
                    "You were logged out, please log in again.";
                context.Result = new RedirectToRouteResult(
                    new RouteValueDictionary(new {controller = "Home", action = "Index"})
                );
                context.Result.ExecuteResultAsync(controller.ControllerContext);
                return -1;
            }
            if (Enum.IsDefined(typeof(Ranks), user.Rank.Name))
            {
                return (int) Enum.Parse(typeof(Ranks), user.Rank.Name);
            }
            return -1;
        }

        private bool FloatingLabelsCompatibleBrowser(HttpContext httpContext)
        {
            string browserName = httpContext.Request.Headers["User-Agent"].ToString().ToLower();
            return !browserName.Contains("edge") &&
                (browserName.Contains("chrome") || browserName.Contains("firefox") || browserName.Contains("safari"));
        }
    }
}
