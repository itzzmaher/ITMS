using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using ITMS.Models;
using ITMS.Repository;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
namespace ITMS.Controllers
{
    public class AccountController : Controller
    {
        AccountRepository AccountRepository = new AccountRepository();
        public IActionResult SignUp()
        {
            return View();
        }
        [HttpPost]
        public IActionResult SignUp(tblUsers userinfo)
        {
            AccountRepository.AddUser(userinfo);
            return View();
        }
        #region Login/Logout
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            try
            {
                string UserName = User.FindFirst(ClaimTypes.Name).Value;
                if (UserName == "" || UserName == null)
                    return View();
                return RedirectToAction("Index", "Home");
            }
            catch
            {
                return View();
            }

        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(tblUsers accountInfo)
        {
            try
            {
                tblUsers Account = AccountRepository.GetAccountsForLogin(accountInfo);
                if (Account == null)
                {
                    ViewData["Login_error"] = "خطأ: اسم المستخدم أو كلمة المرور غير صحيحة";
                    return View();
                }
                else
                {
                    var identity = new ClaimsIdentity(new[]
                    {
                    new Claim(ClaimTypes.Email, Account.Email),
                    new Claim(ClaimTypes.Role, Account.Role.RoleName),
                    new Claim(ClaimTypes.NameIdentifier, Account.Id.ToString()),
                    new Claim(ClaimTypes.Name, Account.GuId.ToString()),
                    new Claim(ClaimTypes.GivenName, Account.Name)

                    }, CookieAuthenticationDefaults.AuthenticationScheme);

                    var principal = new ClaimsPrincipal(identity);

                    await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        principal);

                    return RedirectToAction("Index", "Home");
                }
            }
            catch
            {
                ViewData["Login_error"] = "خطأ: اسم المستخدم أو كلمة المرور غير صحيحة";
                return View();
            }

        }
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction(nameof(AccountController.Login), "Account");
        }


        #endregion Login/Logout

    }
}
