using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITMS.Models
{
    public class tblPlaces
    {
        public int Id { get; set; }
        public Guid GuId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreationDate { get; set; }
        public string ImgName { get; set; }
        public bool IsDeleted { get; set; }
        public int CategoryId { get; set; }
        public tblCategory Category { get; set; }
        public int CityId { get; set; }
        public tblCity City { get; set; }
    }
}
