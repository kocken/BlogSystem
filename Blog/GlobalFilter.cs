using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Blog
{
    public class GlobalFilter : IActionFilter
    {

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var controller = context.Controller as Controller;
            string path = context.HttpContext.Request.Path.ToString().ToLower();
            if (path.EndsWith("/login") || path.EndsWith("/register")) // floating label pages
            {
                controller.ViewBag.ShowFloatingLabels = FloatingLabelsCompatibleBrowser(context.HttpContext);
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            //code here runs after the action method executes
        }

        private bool FloatingLabelsCompatibleBrowser(HttpContext httpContext)
        {
            string browserName = httpContext.Request.Headers["User-Agent"].ToString().ToLower();
            return !browserName.Contains("edge") &&
                (browserName.Contains("chrome") || browserName.Contains("firefox") || browserName.Contains("safari"));
        }
    }
}
