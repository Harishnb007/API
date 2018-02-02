using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Services.Models.LC_WebApi_Models
{

    public class Banks_GetBankList
    {
           public Class1[] Property1 { get; set; }
    }

    public class Class1
    {
        public int userRowID { get; set; }
       
        public bool isNew { get; set; }
        public bool skipChildrenRead { get; set; }
        public string bankName { get; set; }
        public string routingNumber { get; set; }
        public string accountNumber { get; set; }
        public string legalName { get; set; }
        public string accountType { get; set; }
        public int id { get; set; }
    }

  }

