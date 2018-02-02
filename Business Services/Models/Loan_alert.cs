using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Services.Models
{
  public class Loan_alert
    {
        public string alert_id { get; set; }
        public string message_body { get; set; }
        public DateTime message_date { get; set; }
        public string message_title { get; set; }
        public string alert_type { get; set; }
        public string read_status { get; set; }
    }
}
