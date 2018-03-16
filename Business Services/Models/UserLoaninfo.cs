using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Services.Models
{
   public class UserLoaninfo
    {
        public int UserId { get; set; }
        public string loan_number { get; set; }
        public int account_status { get; set; }
        public string property_address { get; set; }
        public string loan_total_amount { get; set; }
        public string loan_duedate { get; set; }
        public decimal loan_principal_balance { get; set; }
        public string loan_notification { get; set; }
        public bool payment_last_received { get; set; }
        public string last_payment_date { get; set; }
        public bool is_enrolled { get; set; }
        public bool is_view_last_statement { get; set; }
        public bool is_payment_past_due { get; set; }
        public bool is_autodraft { get; set; }
        public DateTime auto_draftdate { get; set; }
        public string last_pending_payments { get; set; }
        public string loan_interest_rate { get; set; }
        public string co_borrower_name { get; set; }
        public decimal escrow_balance { get; set; }
        public bool property_value { get; set; }
        public string origination_date { get; set; }
        public bool original_loan_amount { get; set; }
        public bool is_escrow_loan { get; set; }
        public bool is_bankruptcy { get; set; }

    }
}
