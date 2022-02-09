using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using ITMS.Models;
using ITMS.Repository.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace ITMS.Repository
{
    public class AccountRepository : BaseContext
    {
        public int AddUser (tblUsers UserInfo)
        {
            try
            {
                tblUsers userInfoByEmail = _context.tblUsers.AsNoTracking().SingleOrDefault(U => U.Email == UserInfo.Email);
                if (userInfoByEmail != null)
                    return 2;  // user is already there
                UserInfo.GuId = Guid.NewGuid();
                UserInfo.Password = Encrypt(UserInfo.Password);
                UserInfo.IsActive = true;
                UserInfo.RoleId = 1;
                _context.Add(UserInfo);
                _context.SaveChanges();
                return 1; // user added succesfully
            }
            catch
            {
                return 0; //Failed
            }
        }
        #region Encryption
        public string Encrypt(string password)
        {
            string salt = "ITMS";
            string GenPass = password + salt;
            using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
            {
                UTF8Encoding utf8 = new UTF8Encoding();
                byte[] data = md5.ComputeHash(utf8.GetBytes(GenPass));
                return Convert.ToBase64String(data);
            }
        }
        #endregion Encryption
        public tblUsers GetAccountsForLogin(tblUsers userinfo)
        {
            return _context.tblUsers.Include(R => R.Role).SingleOrDefault(U => U.Email == userinfo.Email && U.Password == Encrypt(userinfo.Password));

        }
        public async Task<int> addCertificateDataAsync(tblGuiderCertificate CerInfo , IFormFile ifile , int userID)
        {
            try
            {
                CerInfo.GuId = Guid.NewGuid();
                CerInfo.UserId = userID;
                var saveimg = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\Images", ifile.FileName);
                var stream = new FileStream(saveimg, FileMode.Create);
                await ifile.CopyToAsync(stream);
                CerInfo.ImgName = ifile.FileName;

                 _context.Add(CerInfo);
                 _context.SaveChanges();
                return 1;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public IEnumerable<tblGuiderCertificate> viewAllWaitingCertificate()
        {
            return _context.tblGuiderCertificate.Include(S => S.Status).Include(U => U.User).Include(C => C.City).Where(SC => SC.StatusId == 1);
        }
        public tblGuiderCertificate getApplicantInfo(Guid id)
        {
            return _context.tblGuiderCertificate.Include(S => S.Status).Include(U => U.User).Include(C => C.City).SingleOrDefault(SC => SC.GuId == id);
        }

        public int ApproveApplication(Guid id)
        {
            try
            {
                tblGuiderCertificate CerInfo = _context.tblGuiderCertificate.SingleOrDefault(SC => SC.GuId == id);
                tblUsers UserInfo = _context.tblUsers.SingleOrDefault(U => U.Id == CerInfo.UserId);
                UserInfo.RoleId = 2;
                _context.Update(UserInfo);
                _context.SaveChanges();
                CerInfo.StatusId = 2;
                _context.Update(CerInfo);
                _context.SaveChanges();
                return 1;
            }
            catch (Exception ss)
            {
                return 0;
            }
        }
        public int DenyApplication(Guid id)
        {
            try
            {
                tblGuiderCertificate CerInfo = _context.tblGuiderCertificate.SingleOrDefault(SC => SC.GuId == id);
                
                CerInfo.StatusId = 3;
                _context.Update(CerInfo);
                _context.SaveChanges();
                return 1;
            }
            catch (Exception ss)
            {
                return 0;
            }
        }
    }
    
}

