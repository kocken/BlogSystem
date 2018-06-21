using Data;
using Domain;
using Microsoft.AspNetCore.Http;
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
                if (_context.Users.Any(u => u.Username.ToLower().Equals(user.Username.ToLower())))
                {
                    if (_context.Users.Any(u => 
                    u.Username.ToLower().Equals(user.Username.ToLower()) && 
                    u.Password.ToLower().Equals(user.Password.ToLower())))
                    {
                        HttpContext.Session.SetString("Username", user.Username);
                        _logger.LogInformation($"User \"{user.Username}\" logged in");
                        TempData["Message"] = "Successfully logged in";
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        _logger.LogInformation($"User failed to login to user \"{user.Username}\"");
                        ModelState.AddModelError("Password", "Invalid account password");
                    }
                }
                else
                {
                    _logger.LogInformation
                        ("User tried to login to account which doesn't exist \"" + user.Username + "\"");
                    ModelState.AddModelError("Username", $"The account \"{user.Username}\" doesn't exist");
                }
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
                            HttpContext.Session.SetString("Username", user.Username);
                            _logger.LogInformation($"User \"{user.Username}\" was registered and logged into");
                            TempData["Message"] = "You successfully created an account! You are now logged in.";
                            return RedirectToAction("Index", "Home");
                        }
                        else
                        {
                            _logger.LogError
                                ($"Saving changes returned <= 0 after updating context with user \"{user.Username}\"");
                            ViewBag.ErrorMessage = "Error: The database didn't register your registration. Try again.";
                        }
                    }
                    else
                    {
                        _logger.LogError($"Default rank \"Member\" was failed to get obtained " +
                            $"from database context during {user.Username}'s registration");
                        ViewBag.ErrorMessage = "Error: The default rank was failed to get obtained. Try again.";
                    }
                }
            }
            ViewBag.ShowFloatingLabels = FloatingLabelsCompatibleBrowser();
            return View(user);
        }

        [HttpGet]
        public IActionResult Logout()
        {
            if (Util.GetUsername(HttpContext.Session, out string username))
            {
                HttpContext.Session.Remove("Username");
                _logger.LogInformation($"User \"{username}\" logged out");
                TempData["Message"] = "Successfully logged out";
            }
            return RedirectToAction("Index", "Home");
        }

        [AcceptVerbs("Get", "Post")]
        public IActionResult IsUsernameAvailable(string username)
        {
            string referer = HttpContext.Request.Headers["Referer"];
            if (!referer.EndsWith("/login") && // avoids running check when logging on, only checks on register
                _context.Users.Any(u => u.Username.ToLower().Equals(username.ToLower())))
            {
                return Json($"The username \"{username}\" is already in use.");
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
