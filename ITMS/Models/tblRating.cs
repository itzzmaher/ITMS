using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITMS.Models
{
    public class tblRating
    {
        public int Id { get; set; }
        public int Rate { get; set; }
        public string Details { get; set; }
        public int PlacesId { get; set; }
        public tblPlaces Places { get; set; }
        public int UserId { get; set; }
        public tblUsers User { get; set; }
        public bool IsDeleted { get; set; }
        public Guid GuId { get; set; }
        public DateTime? DateTime { get; set; }
    }
}
