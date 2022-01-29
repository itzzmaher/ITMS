using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITMS.Models
{
    public class tblUsers
    {
        public int Id { get; set; }
        public Guid GuId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Phone { get; set; }
        public tblRoles Role { get; set; }
        public int RoleId { get; set; }
        public bool IsActive { get; set; }

    }
}
