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

    }
    
}

