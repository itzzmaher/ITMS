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
        public IActionResult Index()
        {
            return View (PlaceRepository.getAllplaces());
        }
        public IActionResult AddPlace()
        {
            ViewData["CityId"] = new SelectList(new PlacesRepository().getAllCities(), "Id", "CityName");
            ViewData["CategoryId"] = new SelectList(new PlacesRepository().getAllCategories(), "Id", "CategoryName");
            return View();
        }
        [HttpPost]
        public IActionResult AddPlace(tblPlaces PlaceInfo, IFormFile ifile)
        {
            string imgExt = Path.GetExtension(PlaceInfo.ImgName);
            Task task = PlaceRepository.addPlaceAsync(PlaceInfo, ifile);
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
            PlaceRepository.EditPlace(PlaceInfo, ifile);
            return RedirectToAction(nameof(Index));
        }
        public IActionResult DeletePlace(Guid id)
        {
            PlaceRepository.deletePlaceAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
