
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DroneWorks.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using System.Diagnostics;
using System.Threading;

namespace DroneWorks.Controllers
{
    public class AccountController : Controller
    {
        private readonly Team116DBContext _context;

        public AccountController(Team116DBContext context)
        {
            _context = context;
        }

        public IActionResult Login(string returnURL)
        {

            returnURL = String.IsNullOrEmpty(returnURL) ? "~/" : returnURL;

            return View(new LoginInput { ReturnURL = returnURL });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login([Bind("Login,Password,ReturnURL")] LoginInput loginInput)
        {
            if (ModelState.IsValid)
            {
               var aUser = await _context.WorksUser.FirstOrDefaultAsync(u =>
               u.Login == loginInput.Login && u.Password == loginInput.Password);

               // var aUser = await _context.WorksUser.FirstOrDefaultAsync(u => u.Login == loginInput.Login && u.Password == loginInput.Password);

                if (aUser != null)
                {

                    var claims = new List<Claim>();

                    //claims.Add(new Claim(ClaimTypes.Name, aUser.UserPk.ToString()));
                   claims.Add(new Claim(ClaimTypes.Name, aUser.FirstName));
                    claims.Add(new Claim(ClaimTypes.PostalCode, aUser.Zip.ToString()));
                    claims.Add(new Claim(ClaimTypes.StreetAddress, aUser.Address));
                    claims.Add(new Claim(ClaimTypes.Country, aUser.Country));
                    claims.Add(new Claim(ClaimTypes.StateOrProvince, aUser.State));
                    claims.Add(new Claim(ClaimTypes.Country, aUser.Country));
                    claims.Add(new Claim(ClaimTypes.Sid, aUser.UserPk.ToString()));
                    claims.Add(new Claim(ClaimTypes.Locality, aUser.City));
                    if (aUser.RoleFk.ToString() == "7878")
                    {
                        String role = "Admin,Customer";
                        string[] roles = role.Split(",");

                        foreach (string roller in roles)
                        {
                            claims.Add(new Claim(ClaimTypes.Role, roller));
                        }
                    }
                    else if (aUser.RoleFk.ToString() == "7879")
                    {
                        claims.Add(new Claim(ClaimTypes.Role, "Customer"));
                    }

                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    var principal = new ClaimsPrincipal(identity);

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                    return Redirect(loginInput?.ReturnURL ?? "~/");
                }


                else
                {
                    ViewData["message"] = "Invalid credentials";
                }
            }

            return View(loginInput);
        }

        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignUp([Bind("Login,Password,FirstName")] WorksUser loginInfo)
        {
            if (ModelState.IsValid)
            {
      
                var aUser = await _context.WorksUser.FirstOrDefaultAsync(u => u.Login == loginInfo.Login);


                if (aUser is null)
                {

                    loginInfo.RoleFkNavigation.RoleName = "Customer";
                    _context.Add(loginInfo);
                    await _context.SaveChangesAsync();

                    TempData["success"] = "Account created";

      
                    return RedirectToAction(nameof(Login));
                }
                else
                {
                    ViewData["message"] = "Choose a different username";
                }
            }

            return View(loginInfo);
        }

        public async Task<RedirectToActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
    }
}