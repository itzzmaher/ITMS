using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ITMS.Models;
using ITMS.Repository.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using ITMS.Repository;

using System.Security.Cryptography;
namespace ITMS.Repository
{
    public class PlacesRepository : BaseContext
    {
        AccountRepository AccountRep = new AccountRepository();
        public int AddPlaceAsync(tblPlaces PlaceInfo, IFormFile ifile)
        {
            try { 
            PlaceInfo.GuId = Guid.NewGuid();
            PlaceInfo.IsDeleted = false;
            var saveimg = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\Images", ifile.FileName);
            var stream = new FileStream(saveimg, FileMode.Create);
            ifile.CopyToAsync(stream);
            PlaceInfo.ImgName = ifile.FileName;
            _context.AddAsync(PlaceInfo);
            _context.SaveChangesAsync();
                //_context.Add(PlaceInfo);
                //_context.SaveChanges();
                return 1;
            }
            catch
            {
                return 0;
            }

        }

        public int EditPlace(tblPlaces PlaceInfo, IFormFile ifile)
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
                place.location = PlaceInfo.location;
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
                return 1;
            }
            catch (Exception ec)
            {
                return 0;
            }
        }
        public int deletePlace(Guid id)
        {
            try
            {
                tblPlaces placeInfo = _context.tblPlaces.SingleOrDefault(S => S.GuId == id);
                placeInfo.IsDeleted = true;
                _context.Update(placeInfo);
                _context.SaveChanges();
                return 1;
            }
            catch (Exception ec)
            {
                return 0;
            }


        }
        public string getAddress (Guid id)
        {
            var location = _context.tblPlaces.SingleOrDefault(P => P.GuId == id);
            return location.location;
        }
        public IEnumerable<tblPlaces> getAllplaces()
        {
           return _context.tblPlaces.Include(U => U.Category).Include(S => S.City).Where(P => P.IsDeleted == false);
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

        public int Addtour (tblTour tourInfo, int userID)
        {
            try
            {
                tblGuiderCertificate guiderinfo = _context.tblGuiderCertificate.SingleOrDefault(G => G.UserId == userID);
                tourInfo.GuId = Guid.NewGuid();
                tourInfo.GuiderId = guiderinfo.Id;
                tourInfo.IsDeleted = false;
                _context.Add(tourInfo);
                _context.SaveChanges();
                return 1;
            }
            catch (Exception ss)
            {
                return 0;
            }
        }
        public IEnumerable<tblTour> viewGuiderTours (Guid id)
        {
            tblGuiderCertificate GuiderInfo = AccountRep.getGuiderByGUID(id);
            return _context.tblTour.Where(T => T.GuiderId == GuiderInfo.Id);
        }
        public tblTour getTourInfo (Guid id)
        {
            return _context.tblTour.Include(P => P.Places).Include(G => G.Guider).Include(U => U.Guider.User).SingleOrDefault(T => T.GuId == id);
        }
        public tblGuiderCertificate getGuiderInfoByTourGUID(Guid id)
        {
            tblTour tourInfo = getTourInfo(id);
            return _context.tblGuiderCertificate.SingleOrDefault(G => G.Id == tourInfo.GuiderId);
        }
        public int RegisterTour (tblTourRegisteration RegInfo)
        {
            try
            {
                RegInfo.GuId = Guid.NewGuid();
                RegInfo.RegStatusId = 1;
                _context.Add(RegInfo);
                _context.SaveChanges();
                return 1;
            }
            catch
            {
                return 0;
            }
        }
        
    }
}
