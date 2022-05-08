using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
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
        PlacesRepository PlaceRepository = new PlacesRepository();
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
        public IActionResult SignUp(tblUsers userinfo , string psw)
        {

            userinfo.Password = psw;
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
                    ViewData["Login_error"] = "Email or Password is incorrect, please try again";
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
                    ViewData["Login_success"] = "You logged in successfully";
                    return View();
                }
            }
            catch
            {
                ViewData["Login_error"] = "Email or Password is incorrect, please try again";
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
            ViewData["UserInfo"] = AccountRepository.GetUserById(int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value));
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ChangePassword(tblUsers UserInfo, string psw)
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
                else {
                    string EncryptedPassword = AccountRepository.Encrypt(psw);
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
        public IActionResult GuiderReapply()
        {
            return View();
        }
        public IActionResult GuiderApplication()
        {
            ViewData["CityId"] = new SelectList(new PlacesRepository().getAllCities(), "Id", "CityName");
            ViewData["LanguageId"] = new SelectList(new PlacesRepository().getAllLanguages(), "Id", "EngName");
            int result = AccountRepository.checkForGuider(int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value));
            if (result != 0)
            {
                return RedirectToAction("GuiderStatus", "Account");
            }
            else
            {
                ViewData["CheckForUserApplication"] = result;
                return View();
            }
        }
        [HttpPost]
        public IActionResult GuiderApplication(tblGuiderCertificate certificateInfo, IFormFile ifile, List<int> lang)
        {
            Task Task = AccountRepository.addCertificateDataAsync(certificateInfo, ifile, int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value), lang);
            int result = AccountRepository.checkForGuider(int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value));
            ViewData["CheckForUserApplication"] = result;
            return View();
        }
        public IActionResult GuiderStatus() {

            ViewData["CertificateLanguage"] = AccountRepository.guiderLanguages(int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value));
            return View(AccountRepository.GuiderInfo(int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value)));
        }
        public IActionResult EmailCheck()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EmailCheck(string email)
        {
            tblUsers UserEmailFound = AccountRepository.checkemailuserforRecoverPassword(email);
            if (UserEmailFound != null)
            {
                Random random = new Random();
                string code = random.Next(100000, 999999).ToString();
                int check = emailSending(UserEmailFound.Email, code);
                if (check == 1)
                {
                    ViewData["Successful"] = "The Code is sent to your Email ";
                    TempData["Code"] = code;
                    TempData.Keep("Code");
                    TempData["Page"] = "ForgatPassword";
                    TempData.Keep("Page");
                    TempData["GuId"] = UserEmailFound.GuId;
                    TempData.Keep("GuId");

                    return RedirectToAction(nameof(Code));
                }
                else
                    ViewData["Falied"] = "The Email is not sent succussfuly";
            }
            else
            {
                ViewData["Falied"] = "Problem in processing request";
                ViewData["NoRedirect"] = "No Redirect";
            }
            return View();
        }
        public IActionResult Code()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Code(string codeInput)
        {

            if (codeInput == TempData["Code"].ToString())
                return RedirectToAction(TempData["Page"].ToString(), "Account");
            else
            {
                TempData.Keep("Code");
                ViewData["Falied"] = "Invalid Code";
            }
            return View();
        }
        public IActionResult ForgatPassword()
        {
            Guid guid = Guid.Parse(TempData["GuId"].ToString());
            tblUsers userInfo = AccountRepository.GetUserByGuId(guid);
            return View(userInfo);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ForgatPassword(tblUsers UserInfo, string psw)
        {
            try
            {
                string EncryptedPassword = AccountRepository.Encrypt(psw);
                UserInfo.Password = EncryptedPassword;
                int CheckResult = AccountRepository.ForgatPassword(UserInfo);
                if (CheckResult == 1)
                    ViewData["Successful"] = "Your password were updated successfully, you can login now";
            }
            catch
            {
                ViewData["Falied"] = "An Error Occurred while processing your request, please try again Later";
            }
            return View();
        }
        #region email
        public int emailSending(string emailTo, string code)
        {
            try
            {
                SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
                client.UseDefaultCredentials = true;
                client.EnableSsl = true;

                client.Credentials = new NetworkCredential("ActivitySystemCCSIT@gmail.com", "CCSIT1234");

                MailMessage mailMessage = new MailMessage();
                mailMessage.From = new MailAddress("ActivitySystemCCSIT@gmail.com");
                mailMessage.To.Add(emailTo);

                mailMessage.Body = "This is the new Password:" + code + "  please change it after your first login.";
                mailMessage.Subject = "Activty reset password";
                client.Send(mailMessage);
                return 1;
            }
            catch 
            {
                ModelState.AddModelError("Exception", "an Error has accured please contact the administration");
                return 0;
            }
        }
        #endregion email
        public IActionResult AddCar(string guid)
        {
            ViewData["FuelId"] = new SelectList(new PlacesRepository().getAllFuel(), "Id", "FuelName");
            return View();
        }
        [HttpPost]
        public IActionResult AddCar(tblCar carInfo)
        {
            carInfo.UserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            int result = PlaceRepository.addCar(carInfo);
            if (result == 1)
                ViewData["Successful"] = "Car added successfully";
            else
                ViewData["Falied"] = "An Error Occurred while processing your request, please try again Later";
            return View();
        }
        public IActionResult CarInfo()
        {
            ViewData["CarInfo"] = PlaceRepository.UserCar(int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value));
            return View();
        }
        public IActionResult Orders()
        {
            return View();
        }
    }
}
