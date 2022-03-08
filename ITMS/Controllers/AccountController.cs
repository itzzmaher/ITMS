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
        public IActionResult Profile()
        {

            return View(AccountRepository.GetUserById(int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value)));
        }
        [HttpPost]
        public IActionResult Profile(tblUsers userinfo)
        {
            int result = AccountRepository.ChangeUserinfo(userinfo);
            if (result == 1)
                ViewData["Successful"] = "User info modified Successfully.";
            else
                ViewData["Failed"] = "An Error Occurred while processing your request, please try again Later";
            return View();
        }
        [HttpPost]
        public IActionResult SignUp(tblUsers userinfo)
        {


                int result = AccountRepository.AddUser(userinfo);
                if (result == 1)
                    ViewData["Successful"] = "User added Successfully.";
                else if (result == 2)
                    ViewData["EmailFound"] = "There is already a user with this email.";
                else if (result == 3)
                    ViewData["PhoneFound"] = "There is already a user with this phone.";
                if (result == 0)
                    ViewData["Failed"] = "An Error Occurred while processing your request, please try again Later";
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
            return RedirectToAction("Index", "Home");
        }


        #endregion Login/Logout
        public IActionResult ChangePassword()
        {

            return View(AccountRepository.GetUserById(int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value)));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ChangePassword(tblUsers UserInfo, string NewPassword, string ConfirmedPassword)
        {
            try
            {
                int id = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
                string OldEncryptedPassword = AccountRepository.Encrypt(UserInfo.Password);
                int CheckResult = AccountRepository.CheckUserPassword(id, OldEncryptedPassword);
                if (CheckResult == 0)
                {
                    ViewBag.Error = 2;
                }
                else if (NewPassword != ConfirmedPassword)
                {
                    ViewBag.Error = 1;
                }
                else
                {
                    string EncryptedPassword = AccountRepository.Encrypt(NewPassword);
                    int check = AccountRepository.ChangePassword(id, EncryptedPassword);
                    if (check == 1)
                        ViewData["Successful"] = "Your Password has been changed. You will be logged out";
                    else
                        ViewData["Falied"] = "An Error Occurred while processing your request, please try again Later";
                }
            }
            catch
            {
                ViewData["Falied"] = "An Error Occurred while processing your request, please try again Later";
            }
            return View();
        }
        public IActionResult GuiderApplication()
        {
            ViewData["CityId"] = new SelectList(new PlacesRepository().getAllCities(), "Id", "CityName");
            ViewData["CheckForUserApplication"] = AccountRepository.checkForGuider(int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value));
            return View();
        }
        [HttpPost]
        public IActionResult GuiderApplication(tblGuiderCertificate certificateInfo, IFormFile ifile)
        {
            Task Task = AccountRepository.addCertificateDataAsync(certificateInfo, ifile, int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value));
            return View();
        }
        public IActionResult GuiderStatus() {

            
            return View(AccountRepository.GuiderInfo(int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value)));
        }


    }
}
