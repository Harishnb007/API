using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Services.Models
{
   public class LoanInfo
    {

        public List<LoanDetails> LoanDetails { get; set; }
    }

    public class LoanDetails {

        public string Property_Address { get; set; }
        public string Loan_Number { get; set; }
    }
}
