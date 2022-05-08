using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITMS.Models
{
    public class tblLangGuider
    {
        public int Id { get; set; }
        public int GuiderId { get; set; }
        public tblGuiderCertificate Guider { get; set; }
        public int LanguageId { get; set; }
        public tblLanguage Language { get; set; }
    }
}
