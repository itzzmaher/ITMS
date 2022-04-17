using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace ITMS.Models
{
    public class SystemContext : DbContext
    {
        public DbSet<tblUsers> tblUsers { get; set; }
        public DbSet<tblRoles> tblRoles { get; set; }
        public DbSet<tblPlaces> tblPlaces { get; set; }
        public DbSet<tblCity> tblCity { get; set; }
        public DbSet<tblCategory> tblCategory { get; set; }
        public DbSet<tblRating> tblRating { get; set; }
        public DbSet<tblGuiderCertificate> tblGuiderCertificate { get; set; }
        public DbSet<tblCertificate_status> tblCertificate_status { get; set; }
        public DbSet<tblTour> tblTour { get; set; }
        public DbSet<tblRegisterationStatus> tblRegisterationStatus { get; set; }
        public DbSet<tblTourRegisteration> tblTourRegisteration { get; set; }
        public DbSet<tblCar> tblCar { get; set; }
        public DbSet<tblFuel> tblFuel { get; set; }
        public DbSet<tblMoment> tblMoments { get; set; }
        public DbSet<tblFile> tblFile { get; set; }
        public DbSet<tblUserVisit> tblUserVisit { get; set; }

        public SystemContext(DbContextOptions<SystemContext> options)
            : base(options)
        {

        }
        public SystemContext()
        {

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
           .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
           .AddJsonFile("appsettings.json")
           .Build();
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("DBCS"));
        }
    }
}

