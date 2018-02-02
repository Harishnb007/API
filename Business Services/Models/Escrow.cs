using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Services.Models
{
    public class Escrow
    {
        public decimal current_escrow_balance { get; set; }
        public DateTime last_escrow_analysis_date { get; set; }
        public decimal current_escrow_advance { get; set; }
        public decimal old_monthly_escrow_payment { get; set; }
        public decimal new_monthly_escrow_payment { get; set; }
        public string mortgage_insurance_company { get; set; }
        public string mortgage_policy_number { get; set; }
        public decimal mortgage_annual_premium_amount { get; set; }
        public DateTime mortgage_next_pmi_due_date { get; set; }
        public List<Tax> taxes { get; set; }
        public List<PropertyInsurance> property_insurance { get; set; }
        public decimal principal_balance { get; set; }
        public string user_rowId { get; set; }
        public string loan_number { get; set; }
        public string loan_type { get; set; }

        // public decimal escrow_shortage_amount { get; set; }
    }

    public class Tax
    {
        public string escrow_tax_type { get; set; }
        public string disbursement_frequency { get; set; }
        public DateTime next_disbursement_due_date { get; set; }
        public decimal next_disbursement_due_amount { get; set; }
        public decimal est_annual_disbursement_amount { get; set; }
        public string tax_parcel_id { get; set; }
    }

    public class PropertyInsurance
    {
        public string property_insurance_type { get; set; }
        public string property_insurance_company { get; set; }
        public string property_insurance_policy_number { get; set; }
        public DateTime expiration_date { get; set; }
        public decimal annual_premium { get; set; }
    }
}
