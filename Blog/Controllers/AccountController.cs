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
            if (ViewBag.Username != null)
            {
                _logger.LogInformation($"User {ViewBag.Username} tried to login while already being logged in");
                TempData["Message"] = "You are already logged in";
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        public IActionResult Login(User user)
        {
            if (ViewBag.Username != null)
            {
                _logger.LogInformation($"User {ViewBag.Username} tried to login while already being logged in");
                TempData["Message"] = "You are already logged in";
                return RedirectToAction("Index", "Home");
            }
            if (ModelState.GetFieldValidationState("Username") != ModelValidationState.Valid ||
                ModelState.GetFieldValidationState("Password") != ModelValidationState.Valid)
            {
                _logger.LogInformation("User tried to login with invalid model values");
                return View(user);
            }
            if (!_context.Users.Any(u => u.Username.ToLower().Equals(user.Username.ToLower())))
            {
                _logger.LogInformation($"User tried to login to account which doesn't exist \"{user.Username}\"");
                ModelState.AddModelError("Username", $"The account \"{user.Username}\" doesn't exist.");
                return View(user);
            }
            User dbUser = _context.Users.SingleOrDefault(u =>
                u.Username.ToLower().Equals(user.Username.ToLower()) &&
                u.Password.ToLower().Equals(user.Password.ToLower()));
            if (dbUser != null)
            {
                HttpContext.Session.SetString("Username", dbUser.Username);
                HttpContext.Session.SetInt32("UserId", dbUser.Id);
                _logger.LogInformation($"User \"{user.Username}\" logged in");
                TempData["Message"] = "Successfully logged in";
                return RedirectToAction("Index", "Home");
            }
            else
            {
                _logger.LogInformation($"User failed to login to user \"{user.Username}\" " +
                    "by typing in the wrong password");
                ModelState.AddModelError("Password", "Invalid account password.");
                return View(user);
            }
        }

        [HttpGet]
        public IActionResult Register()
        {
            if (ViewBag.Username != null)
            {
                _logger.LogInformation($"User {ViewBag.Username} tried to register a new account" +
                    " while already being logged in");
                TempData["Message"] = "You are already logged in";
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        public IActionResult Register(User user)
        {
            if (ViewBag.Username != null)
            {
                _logger.LogInformation($"User {ViewBag.Username} tried to register a new account" +
                    " while already being logged in");
                TempData["Message"] = "You are already logged in";
                return RedirectToAction("Index", "Home");
            }
            if (ModelState.GetFieldValidationState("Username") != ModelValidationState.Valid ||
                ModelState.GetFieldValidationState("Password") != ModelValidationState.Valid)
            {
                _logger.LogInformation("User tried to register with invalid model values");
                return View(user);
            }
            if (_context.Users.Any(u => u.Username.ToLower().Equals(user.Username.ToLower())))
            {
                _logger.LogInformation("User tried to register account " +
                    $"with the already existing username \"{user.Username}\"");
                ModelState.AddModelError("Username", $"The username \"{user.Username}\" is already in use.");
                return View(user);
            }
            Rank defaultRank = _context.Ranks.SingleOrDefault(r => r.Name.Equals(Ranks.Member.ToString()));
            if (defaultRank == null)
            {
                _logger.LogError("Default rank \"Member\" was failed to get obtained " +
                    $"from database context during {user.Username}'s registration");
                ViewBag.ErrorMessage = "Error: The default rank was failed to get obtained. Try again.";
                return View(user);
            }
            user.Rank = defaultRank;
            user.JoinTime = DateTime.Now;
            _context.Add(user);
            if (_context.SaveChanges() > 0)
            {
                HttpContext.Session.SetString("Username", user.Username);
                HttpContext.Session.SetInt32("UserId", user.Id);
                _logger.LogInformation($"User \"{user.Username}\" was registered and logged into");
                TempData["Message"] = "You successfully created an account! You are now logged in.";
                return RedirectToAction("Index", "Home");
            }
            else
            {
                _logger.LogError("Saving changes returned <= 0 after adding user " +
                    $"\"{user.Username}\" to context");
                ViewBag.ErrorMessage = "Error: The database didn't register your registration. Try again.";
                return View(user);
            }
        }

        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("Username");
            HttpContext.Session.Remove("UserId");
            if (ViewBag.Username != null)
            {
                _logger.LogInformation($"User \"{ViewBag.Username}\" logged out");
                TempData["Message"] = "Successfully logged out";
            }
            else
            {
                _logger.LogInformation("User tried to logout without being logged in");
            }
            return RedirectToAction("Index", "Home");
        }

        [AcceptVerbs("Get", "Post")]
        public IActionResult IsUsernameAvailable(string username)
        {
            string referer = HttpContext.Request.Headers["Referer"].ToString().ToLower();
            if (!referer.EndsWith("/login") && // avoids running check when logging on, only checks on register
                _context.Users.Any(u => u.Username.ToLower().Equals(username.ToLower())))
            {
                return Json($"The username \"{username}\" is already in use.");
            }
            return Json(true);
        }
    }
}
