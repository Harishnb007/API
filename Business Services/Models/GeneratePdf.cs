using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Services.Models
{
  public class GeneratePdf
    {
        public string Token { get; set; }
        public string date { get; set; }
        public string Key { get; set; }
        public string LoanNumber { get; set; }
    }
}
