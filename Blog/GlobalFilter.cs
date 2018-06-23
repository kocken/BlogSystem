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
            int? userId = context.HttpContext.Session.GetInt32("UserId");
            controller.ViewBag.Username = username;
            controller.ViewBag.UserId = userId;
            GetAccountInfo(context, controller, username, out User user, out int rankLevel);
            controller.ViewBag.User = user;
            controller.ViewBag.RankLevel = rankLevel;
            if (rankLevel >= 1)
            {
                controller.ViewBag.AvailableEvaluations = _context.Comments
                    .IgnoreQueryFilters()
                    .Any(c => c.Evaluations.Count == 0);
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            //code here runs after the action method executes
        }

        private void GetAccountInfo(ActionExecutingContext context, Controller controller, string username,
            out User user, out int rankLevel)
        {
            if (username == null)
            {
                rankLevel = -1;
                user = null;
                return;
            }
            user = _context.Users
                .Include(u => u.Rank)
                .SingleOrDefault(u => u.Username.Equals(username));
            if (user != null && Enum.IsDefined(typeof(Ranks), user.Rank.Name))
            {
                rankLevel = (int)Enum.Parse(typeof(Ranks), user.Rank.Name);
                return;
            }
            else if (user == null)
            {
                context.HttpContext.Session.Remove("Username");
                _logger.LogError($"Logged in user \"{username}\" was not found in the database");
                controller.TempData["Message"] = "Error: The account you are logged into was not found in the database. " +
                    "You were logged out, please log in again.";
                context.Result = new RedirectToRouteResult(
                    new RouteValueDictionary(new {controller = "Home", action = "Index"})
                );
                context.Result.ExecuteResultAsync(controller.ControllerContext);
            }
            rankLevel = -1;
        }

        private bool FloatingLabelsCompatibleBrowser(HttpContext httpContext)
        {
            string browserName = httpContext.Request.Headers["User-Agent"].ToString().ToLower();
            return !browserName.Contains("edge") &&
                (browserName.Contains("chrome") || browserName.Contains("firefox") || browserName.Contains("safari"));
        }
    }
}
