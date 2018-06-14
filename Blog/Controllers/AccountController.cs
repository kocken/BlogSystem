using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;

namespace Blog.Controllers
{
    public class AccountController : Controller
    {
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
                //user.Rank = Ranks.Member; grab from DbContext
                user.JoinTime = DateTime.Now;
                return RedirectToAction("Index", "Home");
            }
            ViewBag.ShowFloatingLabels = FloatingLabelsCompatibleBrowser();
            return View(user);
        }

        public IActionResult Logout()
        {
            return RedirectToAction("Index", "Home");
        }

        private bool FloatingLabelsCompatibleBrowser()
        {
            string browserName = Request.Headers["User-Agent"].ToString().ToLower();
            return !browserName.Contains("edge") &&
                (browserName.Contains("chrome") || browserName.Contains("firefox") || browserName.Contains("safari"));
        }
    }
}
