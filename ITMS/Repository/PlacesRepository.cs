using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ITMS.Models;
using ITMS.Repository.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
namespace ITMS.Repository
{
    public class PlacesRepository : BaseContext
    {
        public async Task addPlaceAsync(tblPlaces PlaceInfo, IFormFile ifile)
        {
            PlaceInfo.GuId = Guid.NewGuid();
            PlaceInfo.IsDeleted = false;
            var saveimg = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\Images", ifile.FileName);
            var stream = new FileStream(saveimg, FileMode.Create);
            await ifile.CopyToAsync(stream);
            PlaceInfo.ImgName = ifile.FileName;

            await _context.AddAsync(PlaceInfo);
            await _context.SaveChangesAsync();
            //_context.Add(PlaceInfo);
            //_context.SaveChanges();

        }
        public IEnumerable<tblPlaces> getAllplaces()
        {
           return _context.tblPlaces.Include(U => U.Category).Include(S => S.City);
        }
        public IEnumerable<tblCity> getAllCities()
        {
            return _context.tblCity;
        }
        public IEnumerable<tblCategory> getAllCategories()
        {
            return _context.tblCategory;
        }
    }
}
