using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Services.Models
{
    public class Loan
    {
        public int account_status { get; set; }
        public string loan_number { get; set; }
        public string property_address { get; set; }
        public decimal loan_total_amount { get; set; }
        public string loan_duedate { get; set; }
        public decimal loan_principal_balance { get; set; }
        public string loan_notification { get; set; } 
        public string loan_type { get; set; }
        public decimal payment_last_received { get; set; }
        public string last_payment_date { get; set; }
        public bool is_enrolled { get; set; }    
        public bool is_view_last_statement { get; set; }
        public bool is_payment_past_due { get; set; }
        public bool is_autodraft { get; set; }
        public string auto_draftdate { get; set; }
        public string last_pending_payments { get; set; }
        public string loan_interest_rate { get; set; }
        public string co_borrower_name { get; set; }
        public decimal escrow_balance { get; set; }
        public decimal property_value { get; set; }
        public string origination_date { get; set; }
        public decimal original_loan_amount { get; set; }
        public string maturity_date { get; set; }
    }
}
