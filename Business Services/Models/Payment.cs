using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Services.Models
{
    public class Payment
    {
        public string loan_number { get; set; }
        public bool account_status { get; set; }
        public decimal payment_amount { get; set; }
        public BankAccount bank_account { get; set; }
        public DateTime payment_date { get; set; }
        public string due_date { get; set; }
        public decimal total_amount { get; set; }
        public string number_of_payments { get; set; }
        public decimal additional_principal { get; set; }
        public decimal additional_escrow { get; set; }
        public bool isAutoDraftSetup { get; set; }
        public decimal late_fees_due { get; set; }
        public string loanType { get; set; }
        public decimal nsf_fees_due { get; set; }
        public decimal other_fees_due { get; set; }
        public decimal onetime_payment_fees { get; set; }
        public decimal principal_balance { get; set; }
        public string userRowId { get; set; }

        public string payment_Type { get; set; }

        public bool override_payment { get; set; }

        public DateTime initial_schDate { get; set; }

        public decimal original_mortgageAmt { get; set; }

        public DateTime date_created { get; set; }
    }
}
