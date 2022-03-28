using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITMS.Models
{
    public class tblTourRegisteration
    {
        public int Id { get; set; }
        public Guid GuId { get; set; }
        public int GuiderId { get; set; }
        public tblGuiderCertificate Guider { get; set; }
        public int UserId { get; set; }
        public tblUsers User { get; set; }
        public int RegStatusId { get; set; }
        public tblRegisterationStatus RegStatus { get; set; }
        public string Details { get; set; }
        public DateTime TourDate { get; set; }

    }
}
