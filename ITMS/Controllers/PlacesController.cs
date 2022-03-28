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
            tblPlaces placeinfo = PlaceRepository.getPlaceInfo(id);

            ViewData["PlaceInfo"] = placeinfo;

            ViewData["AverageRating"] = PlaceRepository.getAverageRating(id);
            try { 
            var lat = findlatitude(placeinfo.location);
            var lon = findlongitude(placeinfo.location);
            ViewData["lon"] = lon;
            ViewData["lat"] = lat;
            }
            catch
            {

            }
            if (ViewData["PlaceInfo"] == null)
            {
                return View();
            }
                else
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

        public IActionResult ViewGuiders()
        {
            return View(AccountRep.viewAllActiveGuiders());
        }
        public IActionResult ViewGuiderInfo (Guid id) {

            ViewData["GuiderTours"] = PlaceRepository.viewGuiderTours(id);
            return View(AccountRep.getApplicantInfo(id));
        }
        public IActionResult TourInfo(Guid id)
        {

            
            return View(PlaceRepository.getTourInfo(id));
        }
        public IActionResult RegisterTour (Guid id)
        {
            tblGuiderCertificate GuiderInfo = PlaceRepository.getGuiderInfoByTourGUID(id);
            ViewData["GID"] = GuiderInfo.Id;
            tblTour tourinfo = PlaceRepository.getTourInfo(id);
            ViewData["Min"] = tourinfo.StartDate;
            ViewData["Max"] = tourinfo.EndDate;
            return View();
        }
        [HttpPost]
        public IActionResult RegisterTour(tblTourRegisteration Registerationinfo)
        {
            Registerationinfo.UserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            int result = PlaceRepository.RegisterTour(Registerationinfo);
            
            return View();
        }
        public double findlongitude(string url)
        {
            double Latitude;

            string rawCoords = url.Split('/').Where(c => c.StartsWith("@") && c.EndsWith("z")).FirstOrDefault();
            var match = Regex.Match(url, @"http.*/@(?<lat>-?\d*\.\d*),(?<lon>-?\d*\.\d*),(?<zzz>\d*z).*");
            Latitude = float.Parse(rawCoords.Split(',')[0].TrimStart('@'));
            var lon1 = match.Groups["lon"].Value;
            return Convert.ToDouble(Latitude);


        }
        public double findlatitude(string url)
        {

            double Longitude;
            string rawCoords = url.Split('/').Where(c => c.StartsWith("@") && c.EndsWith("z")).FirstOrDefault();
            var match = Regex.Match(url, @"http.*/@(?<lat>-?\d*\.\d*),(?<lon>-?\d*\.\d*),(?<zzz>\d*z).*");
            var lat1 = match.Groups["lat"].Value;
            Longitude = float.Parse(rawCoords.Split(',')[1]);
            return Convert.ToDouble(Longitude);
        }
    }
}
