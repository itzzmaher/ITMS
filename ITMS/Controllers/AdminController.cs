using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ITMS.Models;
using ITMS.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ITMS.Controllers
{
    public class AdminController : Controller
    {
        PlacesRepository PlaceRepository = new PlacesRepository();
        AccountRepository AccountRep = new AccountRepository();

        public IActionResult ModifyPlaces()
        {
            return View (PlaceRepository.getAllplaces());
        }
        public IActionResult GasPrice()
        {
            ViewData["95Price"] = PlaceRepository.get95Price();
            ViewData["91Price"] = PlaceRepository.get91Price();
            return View();
        }
        [HttpPost]
        public IActionResult GasPrice(float Price95, float Price91)
        {
            int result = PlaceRepository.updateGasPrices(Price95, Price91);
            if (result == 1)
                ViewData["Successful"] = "Gas prices updated successfully";
            else
                ViewData["Falied"] = "An Error Occurred while processing your request, please try again Later";
            return View();
        }
        public IActionResult AllTourists()
        {
            return View(AccountRep.viewAllTourists());
        }
        public IActionResult AllGuiders()
        {
            ViewData["TodayDate"] = DateTime.Now.ToString("yyyy/MMMM/dd");
            return View(AccountRep.viewAllGuiders());
        }
        public IActionResult Dashboard()
        {
            ViewData["TodayName"] = DateTime.Now.DayOfWeek.ToString();
            ViewData["TodayDate"] = DateTime.Now.ToString("dd/MM/yyy");
            ViewData["AllGuiders"] = AccountRep.viewAllGuiders().Count();
            ViewData["AllTourists"] = AccountRep.viewAllTourists().Count();
            ViewData["WaitingGuiders"] = AccountRep.viewAllWaitingCertificate().Count();
            ViewData["AllPlaces"] = PlaceRepository.getAllplaces().Count();
            ViewData["95Price"] = PlaceRepository.get95Price();
            ViewData["91Price"] = PlaceRepository.get91Price();
            return View();
        }
        public IActionResult AddPlace()
        {
            ViewData["CityId"] = new SelectList(new PlacesRepository().getAllCities(), "Id", "CityName");
            ViewData["CategoryId"] = new SelectList(new PlacesRepository().getAllCategories(), "Id", "CategoryName");
            return View();
        }
        [HttpPost]
        public IActionResult AddPlace(tblPlaces PlaceInfo, IFormFile ifile )
        {
            int result = PlaceRepository.AddPlaceAsync(PlaceInfo, ifile);
            if (result == 1)
                ViewData["Successful"] = "Place Added Successfully";
            else
                ViewData["Falied"] = "An Error Occurred while processing your request, please try again Later";
            return View();
        }
        public IActionResult EditPlace(Guid id)
        {
            ViewData["CityId"] = new SelectList(new PlacesRepository().getAllCities(), "Id", "CityName");
            ViewData["CategoryId"] = new SelectList(new PlacesRepository().getAllCategories(), "Id", "CategoryName");
            return View(PlaceRepository.getPlaceInfo(id));
        }
        [HttpPost]
        public IActionResult EditPlace(tblPlaces PlaceInfo, IFormFile ifile)
        {
            string imgExt = Path.GetExtension(PlaceInfo.ImgName);
            int result = PlaceRepository.EditPlace(PlaceInfo, ifile);
            if (result == 1)
                ViewData["Successful"] = "Place info was modified Successfully";
            else
                ViewData["Falied"] = "An Error Occurred while processing your request, please try again Later";
            return View();
        }
        public IActionResult DeletePlace(Guid id)
        {
            int result = PlaceRepository.deletePlace(id);
            if (result == 1)
            {
                ViewData["Successful"] = "Place info was deleted Successfully";
                return RedirectToAction("Index", "Admin");
            }
            else
            {
                ViewData["Falied"] = "An Error Occurred while processing your request, please try again Later";
                return RedirectToAction("Index", "Admin");
            }

        }

        public IActionResult ApproveCertificate()
        {
            ViewData["CurrentDate"] = DateTime.Now;
             return View(AccountRep.viewAllWaitingCertificate());
        }
        public IActionResult ApplicantInfo(Guid id)
        {
            ViewData["CurrentDate"] = DateTime.Now.ToString("yyyy/MMMM/dd");
            ViewData["CertificateLanguage"] = AccountRep.guiderLanguagesByGUID(id);
            return View(AccountRep.getApplicantInfo(id));
        }
        public IActionResult Approve (Guid id)
        {
            int result = AccountRep.ApproveApplication(id);
            if (result == 1)
                ViewData["Successful"] = "User has been approved Successfully";
            else
                ViewData["Falied"] = "An Error Occurred while processing your request, please try again Later";

            return RedirectToAction("ApproveCertificate");
        }
        public IActionResult Deny(Guid id)
        {
            int result = AccountRep.DenyApplication(id);
            if (result == 1)
                ViewData["Successful"] = "User has been denied Successfully";
            else
                ViewData["Falied"] = "An Error Occurred while processing your request, please try again Later";
            return RedirectToAction("ApproveCertificate");
        }
    }
}
