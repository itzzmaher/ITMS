using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITMS.Models
{
    public class tblCar
    {
        public int Id { get; set; }
        public int FuelEco { get; set; }
        public int UserId { get; set; }
        public tblUsers User { get; set; }
        public int FuelId { get; set; }
        public tblFuel Fuel { get; set; }
    }
}
    
