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

namespace ITMS.Controllers
{
    public class PlacesController : Controller
    {
        PlacesRepository PlaceRepository = new PlacesRepository();
        public IActionResult Index()
        {
            ViewData["Places"] = PlaceRepository.getAllplaces();
            return View();
        }
    }
}
