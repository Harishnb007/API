using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Services.Models
{
  public class AutodraftInsert
    {
        public string legal_name { get; set; }

        public string account_number { get; set; }

        public string account_type { get; set; }

        public string additional_principal { get; set; }

        public string bank_name { get; set; }

        public string draft_payment_on { get; set; }

        public string autodraft_startdate { get; set; }

        public string loan_Number { get; set; }

        public string routing_number { get; set; }

        public string mortgage_amount { get; set; }

        public string total_amount { get; set; }
    }
}
