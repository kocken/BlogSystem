using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Blog.Models;
using Domain;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Blog.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Login()
        {
            ViewBag.ShowFloatingLabels = FloatingLabelsCompatibleBrowser();
            return View();
        }

        public IActionResult Logout()
        {
            return RedirectToAction("Index");
        }

        public IActionResult Register()
        {
            ViewBag.ShowFloatingLabels = FloatingLabelsCompatibleBrowser();
            return View();
        }

        public IActionResult UserCreate(User user)
        {
            if (ModelState.GetFieldValidationState("Username") == ModelValidationState.Valid &&
                ModelState.GetFieldValidationState("Password") == ModelValidationState.Valid)
            {
                //user.Rank = Ranks.Member; grab from DbContext
                user.JoinTime = DateTime.Now;
                return RedirectToAction("Index");
            }
            return RedirectToAction("Register");
        }

        public IActionResult UserLogin(User user)
        {
            if (ModelState.GetFieldValidationState("Username") == ModelValidationState.Valid &&
                ModelState.GetFieldValidationState("Password") == ModelValidationState.Valid)
            {
                return RedirectToAction("Index");
            }
            return RedirectToAction("Login");
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private bool FloatingLabelsCompatibleBrowser()
        {
            string browserName = Request.Headers["User-Agent"].ToString().ToLower();
            return !browserName.Contains("edge") && 
                (browserName.Contains("chrome") || browserName.Contains("firefox") || browserName.Contains("safari"));
        }
    }
}
