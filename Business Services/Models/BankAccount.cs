using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Services.Models
{
    public class BankAccount
    {
        public string account_number { get; set; }
        public string account_nickname { get; set; }
        public string routing_number { get; set; }
        public string bank_name { get; set; }
        public string account_type { get; set; }
        public string legal_name { get; set; }
        public bool isAuthorized { get; set; }

        public bool isNew { get; set; }
    }
}
