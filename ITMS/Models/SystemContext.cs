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

