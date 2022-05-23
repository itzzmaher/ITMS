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
        public int addCar(tblCar carInfo)
        {
            try
            {
                _context.Update(carInfo);
                _context.SaveChanges();
                return 1;
            }
            catch 
            {
                return 0;
            }
        }
        public int updateCar(tblCar carInfo,int userid)
        {
            try
            {
                tblCar car = _context.tblCar.SingleOrDefault(A => A.UserId == userid);
                car.FuelEco = carInfo.FuelEco;
                car.FuelId = carInfo.FuelId;
                car.Name = carInfo.Name;
                _context.Update(car);
                _context.SaveChanges();
                return 1;
            }
            catch
            {
                return 0;
            }
        }
        public IEnumerable<tblFuel> getAllFuel()
        {
            return _context.tblFuel;
        }
        public IEnumerable<tblTour> getAllTours()
        {

            return _context.tblTour.Where(A=> A.IsDeleted == false && A.StartDate < DateTime.Now && A.EndDate > DateTime.Now);
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
            catch 
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
            catch 
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
        public IEnumerable<tblLanguage> getAllLanguages()
        {
            return _context.tblLanguage;
        }
        public IEnumerable<tblCategory> getAllCategories()
        {
            return _context.tblCategory;
        }
        public float get95Price()
        {
            tblFuel fuelInfo = _context.tblFuel.SingleOrDefault(F => F.FuelName == "95");
            return fuelInfo.Price;
        }
        public float get91Price()
        {
            tblFuel fuelInfo = _context.tblFuel.SingleOrDefault(F => F.FuelName == "91");
            return fuelInfo.Price;
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
            catch 
            {
                return 0;
            }
        }
        public int updateTour(tblTour tourInfo)
        {
            try
            {
                tblTour tourdetails = getTourInfo(tourInfo.GuId);
                tourdetails.Description = tourInfo.Description;
                tourdetails.EndDate = tourInfo.EndDate;
                tourdetails.StartDate = tourInfo.StartDate; 
                tourdetails.MaxTourist = tourInfo.MaxTourist;
                tourdetails.Price = tourInfo.Price;
                tourdetails.TourName = tourInfo.TourName;
                _context.Update(tourdetails);
                _context.SaveChanges();
                return 1;
            }
            catch
            {
                return 0;
            }
        }
        public IEnumerable<tblTour> ViewPersonalTours(int id)
        {
            tblGuiderCertificate GuiderInfo = _context.tblGuiderCertificate.SingleOrDefault(G => G.UserId == id);
            return _context.tblTour.Include(P => P.Places).AsNoTracking().Include(G => G.Guider).Where(S => S.GuiderId == GuiderInfo.Id && S.IsDeleted == false );

        }
        public IEnumerable<tblTour> viewGuiderTours (Guid id)
        {
            tblGuiderCertificate GuiderInfo = AccountRep.getGuiderByGUID(id);
            return _context.tblTour.Where(T => T.GuiderId == GuiderInfo.Id && T.IsDeleted == false && T.StartDate < DateTime.Now && T.EndDate > DateTime.Now);
        }
        public int checkYourTour(int user,Guid id)
        {
            tblGuiderCertificate reginfo = _context.tblGuiderCertificate.SingleOrDefault(A => A.UserId == user);
            IEnumerable<tblTour> tour = _context.tblTour.Where(T => T.GuiderId == reginfo.Id);
            if (tour.Count() == 0)
                return 1;
            else
                return 0;
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
        public int DeleteTour (Guid id)
        {
            tblTour tourInfo = _context.tblTour.SingleOrDefault(T => T.GuId == id);
            tourInfo.IsDeleted = true;
            _context.Update(tourInfo);
            _context.SaveChanges();
            return 1;
        }
        public tblCar getUserCar(int id)
        {
            tblCar carinfo = _context.tblCar.Include(F => F.Fuel).AsNoTracking().FirstOrDefault(U => U.UserId == id);

            if (carinfo == null)
                return null;
            else
                return carinfo;
        }
        public IEnumerable<tblTourRegisteration> ViewRegisterOrders(int id)
        {
            tblGuiderCertificate GuiderInfo = _context.tblGuiderCertificate.SingleOrDefault(G => G.UserId == id);
            return _context.tblTourRegisteration.AsNoTracking().Include(G => G.Tour.Places).Include(U => U.User).Where(S => S.Tour.GuiderId == GuiderInfo.Id && S.RegStatusId == 1 || S.RegStatusId == 2);
        }
        public void CancelOrder (Guid id)
        {
            tblTourRegisteration regInfo = getTourRegByGUID(id);
            regInfo.RegStatusId = 3;
            _context.Update(regInfo);
            _context.SaveChanges();
        }
        public void CompleteOrder(Guid id)
        {
            tblTourRegisteration regInfo = getTourRegByGUID(id);
            regInfo.RegStatusId = 5;
            _context.Update(regInfo);
            _context.SaveChanges();
        }
        public void ApproveOrder(Guid id)
        {
            tblTourRegisteration regInfo = getTourRegByGUID(id);
            regInfo.RegStatusId = 2;
            _context.Update(regInfo);
            _context.SaveChanges();
        }
        public void DenyOrder(Guid id)
        {
            tblTourRegisteration regInfo = getTourRegByGUID(id);
            regInfo.RegStatusId = 3;
            _context.Update(regInfo);
            _context.SaveChanges();
        }
        public tblTourRegisteration getTourRegByGUID(Guid id)
        {
            return _context.tblTourRegisteration.SingleOrDefault(R => R.GuId == id);
        }
        public int updateGasPrices (float Price95, float Price91)
        {
            try
            {
                tblFuel fuelInfo = _context.tblFuel.SingleOrDefault(F => F.FuelName == "95");
                fuelInfo.Price = Price95;
                _context.Update(fuelInfo);
                _context.SaveChanges();
                fuelInfo = _context.tblFuel.SingleOrDefault(F => F.FuelName == "91");
                fuelInfo.Price = Price91;
                _context.Update(fuelInfo);
                _context.SaveChanges();
                return 1;
            }
            catch
            {
                return 0;
            }
        }
        public tblMoment getMomentByGUID(Guid id)
        {
            return _context.tblMoments.SingleOrDefault(M => M.GuId == id);
        }
        public int addMoment(tblMoment MomentInfo, List<IFormFile> ifile)
        {
            try
            {
                MomentInfo.GuId = Guid.NewGuid();
                MomentInfo.IsDeleted = false;
                MomentInfo.CreationDate = DateTime.Now;
                _context.Add(MomentInfo);
                _context.SaveChanges();
                tblMoment mInfo = getMomentByGUID(MomentInfo.GuId);
                var id = mInfo.Id;

                foreach (var item in ifile)
                {
                    tblFile FileInfo = new tblFile();
                    var saveimg = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\Images", item.FileName);
                    var stream = new FileStream(saveimg, FileMode.Create);
                    item.CopyToAsync(stream);
                    FileInfo.FileName = item.FileName;
                    FileInfo.MomentId = id;
                    FileInfo.Type = System.IO.Path.GetExtension(item.FileName);
                    _context.AddAsync(FileInfo);
                    
                }
                _context.SaveChangesAsync();
                return 1;
            }
            catch
            {
                return 0;
            }
        }
        public IEnumerable<tblMoment> GetAllMoments()
        {
            return _context.tblMoments.Include(U => U.User).Where(A => A.IsDeleted == false);
        }
        public IEnumerable<tblFile> GetAllFilesMoments( )
        {
            return _context.tblFile.Include(M => M.Moment).Where(A => A.Moment.IsDeleted == false );
        }
        public IEnumerable<tblMoment> GetPerosnalMoments(int userID)
        {
            return _context.tblMoments.Include(U => U.User).Where(A => A.IsDeleted == false && A.UserId == userID);
        }
        public IEnumerable<tblFile> GetPerosnalFilesMoments(int userID)
        {
            return _context.tblFile.Include(M => M.Moment).Where(A => A.Moment.IsDeleted == false && A.Moment.UserId == userID);
        }
        public int addVisit(Guid id,int userID)
        {
            try
            {
                tblPlaces placeinfo = getPlaceInfo(id);
                tblUserVisit VisitInfo = new tblUserVisit();
                VisitInfo.PlacesId = placeinfo.Id;
                VisitInfo.VisitDate = DateTime.Now;
                VisitInfo.UserId = userID;
                _context.Add(VisitInfo);
                _context.SaveChanges();
                return 1;
            }
            catch
            {
                return 0;
            }
        }
        public int checkVisit (int userID , Guid id)
        {
            var check = 0;
            tblPlaces placeInfo = getPlaceInfo(id);
            List< tblUserVisit> userVisits = _context.tblUserVisit.Where(U => U.UserId == userID && U.PlacesId == placeInfo.Id).ToList();
            if (userVisits == null)
                return 0;
            foreach(var item in userVisits)
            {
                //DateTime newdate = item.VisitDate.AddDays(1);
                if (item.VisitDate.AddDays(1) > DateTime.Now)
                    check =  1;
            }
            return check;
        }
        public int checkRating(int userID, Guid id)
        {
            tblPlaces placeInfo = getPlaceInfo(id);
            List<tblRating> userVisits = _context.tblRating.Where(U => U.UserId == userID && U.PlacesId == placeInfo.Id).ToList();
            return userVisits.Count();
        }
        
        public string VisitsCount(Guid id)
        {
            tblPlaces placeInfo = getPlaceInfo(id);
            int  number =  _context.tblUserVisit.Where(U => U.PlacesId == placeInfo.Id).Count();
            if (number == 0)
                return placeInfo.Name +" has no visits recorded";
            else if (number == 1)
                return placeInfo.Name +" has one visit recorded";
            else
                return placeInfo.Name + " has " + number + " visits recorded";
        }
        public tblCar UserCar (int id)
        {
            tblCar carInfo = _context.tblCar.SingleOrDefault(C => C.UserId == id);
            if (carInfo != null)
                return carInfo;
            else
                return null;
        }
       public IEnumerable<tblTourRegisteration> personalOrder(int userId)
        {
            return _context.tblTourRegisteration.Include(R => R.RegStatus).Include(A => A.Tour).Include(A => A.Tour.Guider.User).Include(A => A.Tour.Places).Where(U => U.UserId == userId);
        }
        public int cancelByUser(Guid id)
        {
            try
            {
                tblTourRegisteration regInfo = getTourRegByGUID(id);
                regInfo.RegStatusId = 4;
                _context.Update(regInfo);
                _context.SaveChanges();
                return 1;
            }
            catch
            {
                return 0;
            }
        }
        public int deleteMoment(Guid id)
        {
            try
            {
                tblMoment momInfo = getMomentByGUID(id);
                momInfo.IsDeleted = true;
                _context.Update(momInfo);
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
