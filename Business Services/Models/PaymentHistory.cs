using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Services.Models
{
    public class PaymentHistory
    {
        public List<History> payhistory { get; set; }
    }
    public class History
    {
        public DateTime payment_date { get; set; }
        public decimal total_amount { get; set; }
        public string payment_description { get; set; }
        public DateTime due_date { get; set; }
        public decimal principal_amount { get; set; }
        public decimal interest_amount { get; set; }
        public decimal escrow_amount { get; set; }
        public decimal escrow_balance { get; set; }
        public decimal principal_balance { get; set; }
        public string fee_amount { get; set; }
        public decimal latecharge_amount { get; set; }
        public decimal misc_paid_amount { get; set; }
        public decimal suspense_amount { get; set; }
    }
}
