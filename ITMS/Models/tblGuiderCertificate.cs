using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITMS.Models
{
    public class tblGuiderCertificate
    {
        public int Id { get; set; }
        public Guid GuId { get; set; }
        public int CityId { get; set; }
        public tblCity City { get; set; }
        public int UserId { get; set; }
        public tblUsers User { get; set; }
        public string ImgName { get; set; }
        public DateTime ExpireDate { get; set; }
        public tblCertificate_status Status { get; set; }
        public int? StatusId { get; set; }

    }
}
