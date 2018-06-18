using Data;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

namespace Blog.Controllers
{
    public class AccountController : Controller
    {
        private readonly BlogContext _context;
        private readonly ILogger _logger;

        public AccountController(BlogContext context, ILogger<AccountController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Login()
        {
            ViewBag.ShowFloatingLabels = FloatingLabelsCompatibleBrowser();
            return View();
        }

        [HttpPost]
        public IActionResult Login(User user)
        {
            if (ModelState.GetFieldValidationState("Username") == ModelValidationState.Valid &&
                ModelState.GetFieldValidationState("Password") == ModelValidationState.Valid)
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.ShowFloatingLabels = FloatingLabelsCompatibleBrowser();
            return View(user);
        }

        [HttpGet]
        public IActionResult Register()
        {
            ViewBag.ShowFloatingLabels = FloatingLabelsCompatibleBrowser();
            return View();
        }

        [HttpPost]
        public IActionResult Register(User user)
        {
            if (ModelState.GetFieldValidationState("Username") == ModelValidationState.Valid &&
                ModelState.GetFieldValidationState("Password") == ModelValidationState.Valid)
            {
                if (_context.Users.Any(u => u.Username.ToLower().Equals(user.Username.ToLower())))
                {
                    _logger.LogInformation
                        ("User tried to register account with already existing username \"" + user.Username + "\"");
                    ModelState.AddModelError("Username", $"The username \"{user.Username}\" is already in use");
                }
                else
                {
                    Rank defaultRank = _context.Ranks.Single(r => r.Name.Equals(Ranks.Member.ToString()));
                    if (defaultRank != null)
                    {
                        user.Rank = defaultRank;
                        user.JoinTime = DateTime.Now;
                        _context.Update(user);
                        if (_context.SaveChanges() > 0)
                        {
                            _logger.LogInformation($"User \"{user.Username}\" was registered");
                            TempData["NewRegistration"] = true;
                            return RedirectToAction("Index", "Home");
                        }
                        else
                        {
                            _logger.LogError($"Saving after updating context with user \"{user}\" returned <= 0");
                            ViewBag.ErrorMessage = "An issue occured, try again (Error code: 1)";
                        }
                    }
                    else
                    {
                        _logger.LogError("Default rank \"Member\" failed to get grabbed from database context");
                        ViewBag.ErrorMessage = "An issue occured, try again (Error code: 2)";
                    }
                }
            }
            ViewBag.ShowFloatingLabels = FloatingLabelsCompatibleBrowser();
            return View(user);
        }

        [HttpGet]
        public IActionResult Logout()
        {
            return RedirectToAction("Index", "Home");
        }

        [AcceptVerbs("Get", "Post")]
        public IActionResult IsUsernameAvailable(string username)
        {
            if (_context.Users.Any(u => u.Username.ToLower().Equals(username.ToLower())))
            {
                return Json($"The username \"{username}\" is already in use");
            }
            return Json(true);
        }

        private bool FloatingLabelsCompatibleBrowser()
        {
            string browserName = Request.Headers["User-Agent"].ToString().ToLower();
            return !browserName.Contains("edge") &&
                (browserName.Contains("chrome") || browserName.Contains("firefox") || browserName.Contains("safari"));
        }
    }
}
