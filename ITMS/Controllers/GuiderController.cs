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
        public IActionResult AddTour(Guid id)
        {
            tblPlaces placeinfo = PlaceRepository.getPlaceInfo(id);
            ViewData["id"] = placeinfo.Id;
            ViewData["ImgName"] = placeinfo.ImgName;

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
    }
}
