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
            TempData.Remove("GuId");
            ViewData["Places"] = PlaceRepository.getAllplaces();
            return View();
        }
        public IActionResult ViewPlace(string id)
        {
            TempData["GuId"] = id;
            TempData.Keep("GuId");
            return RedirectToAction(nameof(PlaceInfo));
        }
        public IActionResult PlaceInfo()
        {

            Guid id = Guid.Parse(TempData["GuId"].ToString());
            TempData.Keep("GuId");
            ViewData["Ratings"] = PlaceRepository.getAllRatingForPlace(id);
            tblPlaces placeinfo = PlaceRepository.getPlaceInfo(id);
            if (User.Identity.IsAuthenticated) { 
            ViewData["CarInfo"] = PlaceRepository.getUserCar(int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value));
            ViewData["UserCheckVisit"] = PlaceRepository.checkVisit(int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value),id);
            ViewData["FuelId"] = new SelectList(new PlacesRepository().getAllFuel(), "Id", "FuelName");
            ViewData["UserRatings"] = PlaceRepository.checkRating(int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value), id);
            }
            ViewData["tours"] = PlaceRepository.getPlaceTours(id);
            ViewData["NumberOfVisists"] = PlaceRepository.VisitsCount(id);
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
            return View();
        }
        [HttpPost]
        public IActionResult AddRating(tblRating rateInfo)
        {
            PlaceRepository.addRate(rateInfo, int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value));
            return RedirectToAction(nameof(PlaceInfo));
        }
        public IActionResult BrowseCategory(string category)
        {
            ViewData["Places"] = PlaceRepository.getPlaceByCategory(category);
            return View();
        }

        public IActionResult ViewGuiders()
        {
            TempData.Remove("GGuId");
            return View(AccountRep.viewAllActiveGuiders());
        }
        public IActionResult AllTours()
        {
            TempData.Remove("TourGuId");
            return View(PlaceRepository.getAllTours());
        }
        public IActionResult Visit(Guid id)
        {
            int reuslt = PlaceRepository.addVisit(id, int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value));

            return RedirectToAction(nameof(PlaceInfo));
        }
        
        public IActionResult ShareMoment()
        {
            return View();
        }

        public IActionResult AddACar(string Name, int FuelEco, int FuelId, Guid id)
        {
            AccountRep.addcar(Name, FuelEco, FuelId, int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value));

            return RedirectToAction(nameof(PlaceInfo));
        }
        
        [HttpPost]
        public IActionResult ShareMoment(tblMoment MomentInfo, List<IFormFile> ifile)
        {
            MomentInfo.UserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            int result = PlaceRepository.addMoment(MomentInfo, ifile);
            if (result == 1)
                ViewData["Successful"] = "The momemnt were shared successfully";
            else
                ViewData["Falied"] = "An Error Occurred while processing your request, please try again Later";
            return View();
        }
        public IActionResult ViewGuider(Guid id)
        {
            TempData["GGuId"] = id;
            TempData.Keep("GGuId");
            return RedirectToAction(nameof(ViewGuiderInfo));
        }
        public IActionResult ViewGuiderInfo () {
            Guid id = Guid.Parse(TempData["GGuId"].ToString());
            TempData.Keep("GGuId");
            ViewData["GuiderTours"] = PlaceRepository.viewGuiderTours(id);
            ViewData["CertificateLanguage"] = AccountRep.guiderLanguagesByGUID(id);
            return View(AccountRep.getApplicantInfo(id));
        }
        public IActionResult Moments()
        {
            TempData.Remove("userID");
            ViewData["AllMoments"] = PlaceRepository.GetAllMoments();
            ViewData["AllFilesMoments"] = PlaceRepository.GetAllFilesMoments();
            return View();
        }
        public IActionResult userMomemnts(Guid id)
        {
            TempData["userID"] = id;
            TempData.Keep("userID");
            ViewData["AllMoments"] = PlaceRepository.GetAllUserMoments(id);
            ViewData["AllFilesMoments"] = PlaceRepository.GetAllUserFilesMoments(id);
            return RedirectToAction(nameof(userMoments));
        }
        public IActionResult userMoments()
        {

            Guid id = Guid.Parse(TempData["userID"].ToString());
            TempData.Keep("userID");
            ViewData["AllMoments"] = PlaceRepository.GetAllUserMoments(id);
            ViewData["AllFilesMoments"] = PlaceRepository.GetAllUserFilesMoments(id);
            return View();
        }

        public IActionResult Tour(Guid id)
        {
            TempData["TourGuId"] = id;
            TempData.Keep("TourGuId");
            return RedirectToAction(nameof(TourInfo));
        }
        public IActionResult TourInfo()
        {
            Guid id = Guid.Parse(TempData["TourGuId"].ToString());
            TempData.Keep("TourGuId");
            if (User.Identity.IsAuthenticated) 
            ViewData["checkAvaliablity"] = PlaceRepository.checkYourTour(int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value), id);
            return View(PlaceRepository.getTourInfo(id));
        }
        public IActionResult RegisterTour ()
        {

            Guid id = Guid.Parse(TempData["TourGuId"].ToString());
            TempData.Keep("TourGuId");
            tblTour tourinfo = PlaceRepository.getTourInfo(id);
            ViewData["TID"] = tourinfo.Id;
            ViewData["tourinfo"] = tourinfo;
            return View();
        }
        [HttpPost]
        public IActionResult RegisterTour(tblTourRegisteration Registerationinfo)
        {
            Registerationinfo.UserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            int result = PlaceRepository.RegisterTour(Registerationinfo);
            return RedirectToAction();
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
