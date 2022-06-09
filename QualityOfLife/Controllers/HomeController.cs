using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using QualityOfLife.Data;
using QualityOfLife.Models;

namespace QualityOfLife.Controllers
{
    public class HomeController : Controller
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        //private readonly ApplicationDbContext _usuario;

        public HomeController(SignInManager<IdentityUser> signInManager)
        {
            _signInManager = signInManager;
        }

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
        public async Task<IActionResult> Logout()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index");
            }
            var signOut = _signInManager.SignOutAsync();

            // Obtem o login
            var user = _signInManager.UserManager.Users.First(x => x.Email == User.Identity.Name);
            // Atualiza sua saida
            //user.RegisteredLogin.Add(new RegisteredLogins { Provider = _application, Action = false });
            await _signInManager.UserManager.UpdateAsync(user);
            await signOut;

            return RedirectToAction("Index");
        }
        //public IActionResult Logout(string returnUrl = null)
        //{
        //    if (!User.Identity.IsAuthenticated)
        //    {
        //        return RedirectToAction("Index");
        //    }
        //    var signOut = _signInManager.SignOutAsync();

        //    // Obtem o login
        //    var user = _signInManager.UserManager.Users.First(x => x.Email == User.Identity.Name);
        //    // Atualiza sua saida
        //    //user.RegisteredLogin.Add(new RegisteredLogins { Provider = _application, Action = false });
        //    //await _signInManager.UserManager.UpdateAsync(user);
        //    //await signOut;

        //    return RedirectToAction("Index");
        //    //return RedirectToAction("/Account/Logout", "Identity");
        //}

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
