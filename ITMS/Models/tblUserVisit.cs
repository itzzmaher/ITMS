using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITMS.Models
{
    public class tblUserVisit
    {
        public int Id { get; set; }
        public DateTime VisitDate { get; set; }
        public int PlacesId { get; set; }
        public tblPlaces Places { get; set; }
        public int UserId { get; set; }
        public tblUsers User { get; set; }
    }
}
