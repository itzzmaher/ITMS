using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITMS.Models
{
    public class tblTour
    {
        public int Id { get; set; }
        public string TourName { get; set; }
        public Guid GuId { get; set; }
        public int PlacesId { get; set; }
        public tblPlaces Places { get; set; }
        public int GuiderId { get; set; }
        public tblGuiderCertificate Guider { get; set; }
        public int Price { get; set; }
        public int MaxTourist { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Description { get; set; }
        public string ImgName { get; set; }
        public bool IsDeleted { get; set; }
    }
}
