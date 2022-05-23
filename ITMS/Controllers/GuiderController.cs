using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ITMS.Models;
using ITMS.Repository;
using Microsoft.AspNetCore.Mvc;

namespace ITMS.Controllers
{
    public class GuiderController : Controller
    {
        AccountRepository AccountRep = new AccountRepository();
        PlacesRepository PlaceRepository = new PlacesRepository();
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Dashboard()
        {
            ViewData["TodayName"] = DateTime.Now.DayOfWeek.ToString();
            ViewData["TodayDate"] = DateTime.Now.ToString("dd/MM/yyy");
            ViewData["Current_Tours"] = PlaceRepository.ViewPersonalTours(int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value)).Count();
            ViewData["Current_Order"] = PlaceRepository.ViewRegisterOrders(int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value)).Count();
            ViewData["Current_Status"] = AccountRep.statusName(int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value));

            return View();
        }
        public IActionResult GuiderStatus()
        {

            ViewData["CertificateLanguage"] = AccountRep.guiderLanguages(int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value));
            return View(AccountRep.GuiderInfo(int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value)));
        }
        public IActionResult Tours()
        {
            
            return View(PlaceRepository.ViewPersonalTours(int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value)));
        }
        public IActionResult UpdateTour(Guid id)
        {
            return View(PlaceRepository.getTourInfo(id));
        }
        [HttpPost]
        public IActionResult UpdateTour(tblTour tourinfo)
        {
            int result = PlaceRepository.updateTour(tourinfo);
            if (result == 1)
                ViewData["Successful"] = "The tour has been updated succesfully";
            else
                ViewData["Falied"] = "An Error Occurred while processing your request, please try again Later";
            return View();
        }
        public IActionResult AddTour(Guid id)
        {
            tblPlaces placeinfo = PlaceRepository.getPlaceInfo(id);
            ViewData["id"] = placeinfo.Id;
            ViewData["ImgName"] = placeinfo.ImgName;
            ViewData["currentDate"] = DateTime.Now;

            return View();
        }
        [HttpPost]
        public IActionResult AddTour(tblTour tourinfo)
        {

            int result = PlaceRepository.Addtour(tourinfo, int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value));
            if (result == 1)
                ViewData["Successful"] = "The tour has been added succesfully";
            else
                ViewData["Falied"] = "An Error Occurred while processing your request, please try again Later";
            return View();
        }
        public IActionResult ApproveOrder(Guid id)
        {
            PlaceRepository.ApproveOrder(id);
            return RedirectToAction("WaitingOrders");
        }
        public IActionResult DenyOrder(Guid id)
        {
            PlaceRepository.DenyOrder(id);
            return RedirectToAction("WaitingOrders");
        }
        public IActionResult CancelOrder(Guid id)
        {
            PlaceRepository.CancelOrder(id);
            return RedirectToAction("WaitingOrders");
        }
        public IActionResult CompleteOrder(Guid id)
        {
            PlaceRepository.CompleteOrder(id);
            return RedirectToAction("WaitingOrders");
        }
        public IActionResult DeleteTour(Guid id)
        {
            int result = PlaceRepository.DeleteTour(id);
            if (result == 1)
                ViewData["Successful"] = "The tour has been added succesfully";
            else
                ViewData["Falied"] = "An Error Occurred while processing your request, please try again Later";
            return RedirectToAction("Tours");
        }
        public IActionResult WaitingOrders()
        {
            return View(PlaceRepository.ViewRegisterOrders(int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value)));
        }
    }
}
