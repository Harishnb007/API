using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Services.Models
{
    public class AutoDraft
    {
        public BankAccount bank_account { get; set; }
        public decimal mortgage_amount { get; set; }
        public decimal total_amount { get; set; }
        public decimal additional_principal { get; set; }
        public int draft_payment_on { get; set; }
        public DateTime autodraft_startdate { get; set; }
        public string draft_delayDays { get; set; }
    }
}
