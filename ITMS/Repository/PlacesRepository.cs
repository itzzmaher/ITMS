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

        public void EditPlace(tblPlaces PlaceInfo, IFormFile ifile)
        {
            try
            {
                tblPlaces place = getPlaceInfo(PlaceInfo.GuId);
                place.CategoryId = PlaceInfo.CategoryId;
                place.CityId = PlaceInfo.CityId;
                place.Name = PlaceInfo.Name;
                place.GuId = PlaceInfo.GuId;
                place.Description = PlaceInfo.Description;
                place.IsDeleted = PlaceInfo.IsDeleted;
                place.CreationDate = PlaceInfo.CreationDate;
                if (place.ImgName != null)
                {
                    
                        var CurrentImage = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\Images", place.ImgName);
                        System.IO.File.Delete(CurrentImage);
                        var saveimg = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\Images", ifile.FileName);
                        var stream = new FileStream(saveimg, FileMode.Create);
                        ifile.CopyToAsync(stream);
                        place.ImgName = ifile.FileName;
                }
                //_context.Entry(place).State = EntityState.Modified;
                //_context.SaveChanges();
                _context.tblPlaces.Update(place);
                _context.SaveChanges();
            }
            catch (Exception ec)
            {

            }
        }
        public void deletePlaceAsync(Guid id)
        {
            tblPlaces placeInfo = _context.tblPlaces.SingleOrDefault(S => S.GuId == id);
            if (placeInfo.ImgName != null)
            {
                var CurrentImage = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\Images", placeInfo.ImgName);
                _context.tblPlaces.Remove(placeInfo);
                if (_context.SaveChanges() > 0)
                {
                    if (System.IO.File.Exists(CurrentImage))
                    {
                        System.IO.File.Delete(CurrentImage);
                    }
                }
            }
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
        public IEnumerable<tblPlaces> getPlaceByCategory(string category)
        {
            return _context.tblPlaces.Include(U => U.Category).Include(S => S.City).Where(P => P.Category.CategoryName == category);
        }
        public tblPlaces getPlaceInfo(Guid guid)
        {
            return _context.tblPlaces.Include(U => U.Category).Include(S => S.City).SingleOrDefault(P => P.GuId == guid);
        }
        public IEnumerable<tblRating> getAllRatingForPlace(Guid guid)
        {
            return _context.tblRating.Include(U => U.User).Include(P => P.Places).Where(R => R.Places.GuId == guid);
        }
        public void addRate (tblRating rateInfo, int userID)
        {
            rateInfo.UserId = userID;
            _context.Add(rateInfo);
            _context.SaveChanges();

        }
        public float getAverageRating(Guid id)
        {
            float sum = 0, i = 0;
            IEnumerable<tblRating> list = getAllRatingForPlace(id);
            foreach (var item in list)
            {
                sum += item.Rate;
                i++;
            }
                return sum / i;
        }
        
    }
}
