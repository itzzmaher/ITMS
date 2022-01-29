using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ITMS.Models;

namespace ITMS.Repository.Common
{
    public class BaseContext
    {
        private SystemContext context;

        internal SystemContext _context
        {
            get
            {
                if (context == null)
                {
                    context = new SystemContext();
                    return context;
                }
                else
                    return context;
            }
        }

    }
}