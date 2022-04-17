using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITMS.Models
{
    public class tblMoment
    {
        public int Id { get; set; }
        public string Details { get; set; }
        public int UserId { get; set; }
        public tblUsers User { get; set; }
        public bool IsDeleted { get; set; }
        public Guid GuId { get; set; }

    }
}
