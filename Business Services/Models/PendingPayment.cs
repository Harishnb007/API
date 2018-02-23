using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Services.Models
{
    public class PendingPayment
    {
        public string payment_id { get; set; }
        public string payment_date { get; set; }
        public decimal? total_amount { get; set; }
        public string payment_description { get; set; }
        public string account_number { get; set; }
        public int paymentCount { get; set; }
        public string date_created { get; set; }
    }
}
