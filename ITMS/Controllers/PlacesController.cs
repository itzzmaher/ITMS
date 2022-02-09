using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ITMS.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using ITMS.Repository;
using System.Security.Claims;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;

namespace ITMS.Controllers
{
    public class PlacesController : Controller
    {
        AccountRepository AccountRep = new AccountRepository();
        PlacesRepository PlaceRepository = new PlacesRepository();
        public IActionResult ViewPlaces()
        {
            ViewData["Places"] = PlaceRepository.getAllplaces();
            return View();
        }

        public IActionResult PlaceInfo(Guid id)
        {
            ViewData["Ratings"] = PlaceRepository.getAllRatingForPlace(id);
            ViewData["PlaceInfo"] = PlaceRepository.getPlaceInfo(id);
            ViewData["AverageRating"] = PlaceRepository.getAverageRating(id);
            return View();
        }
        [HttpPost]
        public IActionResult AddRating(tblRating rateInfo)
        {
            PlaceRepository.addRate(rateInfo, int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value));
            return RedirectToAction("ViewPlaces");
        }
        public IActionResult BrowseCategory(string category)
        {
            ViewData["Places"] = PlaceRepository.getPlaceByCategory(category);
            return View();
        }
        [Authorize(Roles = "Admin,SuperAdmin")]
        public IActionResult GuiderApplication()
        {
            ViewData["CityId"] = new SelectList(new PlacesRepository().getAllCities(), "Id", "CityName");
            return View();
        }
        [HttpPost]
        public IActionResult GuiderApplication(tblGuiderCertificate certificateInfo , IFormFile ifile)
        {
            Task Task = AccountRep.addCertificateDataAsync(certificateInfo, ifile, int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value));
            return View();
        }

    }
}
