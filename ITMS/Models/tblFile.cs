using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITMS.Models
{
    public class tblFile
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public int MomentId { get; set; }
        public tblMoment Moment { get; set; }
        public string? Type { get; set; }
    }
}
