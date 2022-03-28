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
using System.Security.Claims;


namespace ITMS.Repository
{
    public class AccountRepository : BaseContext
    {
        public int AddUser(tblUsers UserInfo)
        {
            try
            {
                tblUsers userInfoByEmail = _context.tblUsers.AsNoTracking().SingleOrDefault(U => U.Email == UserInfo.Email);
                if (userInfoByEmail != null)
                    return 2;  // user is already this email
                tblUsers userInfoByNumber = _context.tblUsers.AsNoTracking().SingleOrDefault(U => U.Phone == UserInfo.Phone);
                if (userInfoByNumber != null)
                    return 3;  // user is already this email
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
                return 0; //Failed to add user
            }
        }
        #region Encryption
        //public string Encrypt(string password)
        //{
        //    string salt = "ITMS";
        //    string GenPass = password + salt;
        //    using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
        //    {
        //        UTF8Encoding utf8 = new UTF8Encoding();
        //        byte[] data = md5.ComputeHash(utf8.GetBytes(GenPass));
        //        return Convert.ToBase64String(data);

        //    }
        //}
        public static string Encrypt(string password)
        {
            string salt = "ITMS";
            string GenPass = password + salt;
            MD5CryptoServiceProvider MD5Code = new MD5CryptoServiceProvider();
            byte[] byteDizisi = Encoding.UTF8.GetBytes(GenPass);
            byteDizisi = MD5Code.ComputeHash(byteDizisi);
            StringBuilder sb = new StringBuilder();
            foreach (byte ba in byteDizisi)
            {
                sb.Append(ba.ToString("x2").ToLower());
            }
            return sb.ToString();
        }
        #endregion Encryption
        public tblUsers GetAccountsForLogin(tblUsers userinfo)
        {
            userinfo.Password = Encrypt(userinfo.Password);
            tblUsers userinf = _context.tblUsers.Include(R => R.Role).SingleOrDefault(U => U.Email == userinfo.Email && U.Password == userinfo.Password);
            return userinf;


        }
        public async Task<int> addCertificateDataAsync(tblGuiderCertificate CerInfo, IFormFile ifile, int userID)
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
        public IEnumerable<tblGuiderCertificate> viewAllActiveGuiders()
        {
            return _context.tblGuiderCertificate.Include(S => S.Status).Include(U => U.User).Include(C => C.City).Where(SC => SC.StatusId == 2);
        }
        public tblGuiderCertificate getApplicantInfo(Guid id)
        {
            return _context.tblGuiderCertificate.Include(S => S.Status).Include(U => U.User).Include(C => C.City).SingleOrDefault(SC => SC.GuId == id);
        }
        public tblGuiderCertificate getGuiderByGUID(Guid id)
        {
            return _context.tblGuiderCertificate.SingleOrDefault(SC => SC.GuId == id);
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
        public tblUsers GetUserByGuId(Guid id)
        {
            return _context.tblUsers.SingleOrDefault(U => U.GuId == id);
        }
        public tblUsers GetUserById(int id)
        {
            return _context.tblUsers.SingleOrDefault(U => U.Id == id);
        }
        public int ChangeUserinfo(tblUsers userinfo)
        {
            try
            {
                tblUsers userInfoByGuid = GetUserByGuId(userinfo.GuId);
                userInfoByGuid.Email = userinfo.Email;
                userInfoByGuid.Name = userinfo.Name;
                userInfoByGuid.Phone = userinfo.Phone;
                _context.Update(userInfoByGuid);
                _context.SaveChanges();
                return 1;
            }
            catch
            {
                return 0;
            }
        }
        public int CheckUserPassword(int id, string Password)
        {
            tblUsers UserInfo = _context.tblUsers.SingleOrDefault(U => U.Password == Password && U.Id == id);
            if (UserInfo == null)
                return 0;
            else
                return 1;
        }
        public int ChangePassword(int AccountID, string password)
        {
            try
            {
                tblUsers UserInfoByID = GetUserById(AccountID);
                UserInfoByID.Password = password;
                _context.Update(UserInfoByID);
                _context.SaveChanges();
                return 1;
            }
            catch
            {
                return 0;
            }
        }
        public int checkForGuider(int Id)
        {
            try
            {
                tblGuiderCertificate RegisterStatus = _context.tblGuiderCertificate.SingleOrDefault(R => R.UserId == Id);
                if (RegisterStatus.StatusId != 3)
                    return 1;
                else
                    return 0;
            }
            catch
            {
                return 0;
            }
        }
        public tblGuiderCertificate GuiderInfo(int Id)
        {
            return _context.tblGuiderCertificate.Include(S => S.Status).Include(U => U.User).Include(C => C.City).SingleOrDefault(R => R.UserId == Id);


        }
    }
}

