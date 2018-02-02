using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Services.Models
{
    class PaymentDetails
    {
        [JsonProperty]
        internal string loan_duedate { get; set; }

        [JsonProperty]
        internal LastRegularPayment last_regular_payment { get; set; }

        [JsonProperty]
        internal NextMonthlyPayment next_monthly_payment { get; set; }
    }

    class LastRegularPayment
    {
        [JsonProperty]
        internal string principal_amount { get; set; }

        [JsonProperty]
        internal string interest_amount { get; set; }

        [JsonProperty]
        internal string last_payment_date { get; set; }

        [JsonProperty]
        internal string escrow_amount { get; set; }

        [JsonProperty]
        internal string total_amount { get; set; }


    }

    class NextMonthlyPayment
    {
        [JsonProperty]
        internal string principal_interest_amount { get; set; }

        [JsonProperty]
        internal string property_insurance_amount { get; set; }

        [JsonProperty]
        internal string city_tax_amount { get; set; }

        [JsonProperty]
        internal string county_tax_amount { get; set; }

        [JsonProperty]
        internal string other_tax_amount { get; set; }

        [JsonProperty]
        internal string monthly_mortgage_insurance_amount { get; set; }

        [JsonProperty]
        internal string overage_shortage { get; set; }

        [JsonProperty]
        internal string total_amount { get; set; }



    }
}
