using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QualityOfLife.Models;

namespace QualityOfLife.Controllers
{
    public class HomeController : Controller
    {

        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                return View();
            }
            return RedirectToAction("/Account/Login", "Identity");
        }

        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Logout(string returnUrl = null)
        {
            return RedirectToAction("/Account/Logout", "Identity");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
