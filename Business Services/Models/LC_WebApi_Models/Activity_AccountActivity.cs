using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Services.Models.LC_WebApi_Models
{
    class Activity_AccountActivity
    {

        [JsonProperty]
        internal List<FullMortageHistory> fullMortageHistory { get; set; }
        // Modified by BBSR Team on 10th Jan
        public List<Activity> ActivityType { get; set; }

    }
    class FullMortageHistory
    {

        [JsonProperty]
        internal string transactionAppliedDate { get; set; }

        [JsonProperty]
        internal decimal totalPaymentReceivedAmount { get; set; }

        [JsonProperty]
        internal string tranCodeDesc { get; set; }

        [JsonProperty]
        internal string feeAmount { get; set; }

        [JsonProperty]
        internal string lateChargeAmount { get; set; }

        [JsonProperty]
        internal string miscPaidAmount { get; set; }

        [JsonProperty]
        internal string suspenseAmount { get; set; }

        [JsonProperty]
        internal decimal unpaidPrincipalBalance { get; set; }

        [JsonProperty]
        internal decimal escrowBalance { get; set; }

        //[JsonProperty]
        //internal string lastPayRecd { get; set; }

        [JsonProperty]
        internal decimal principalPmtAmount { get; set; }

        [JsonProperty]
        internal decimal escrowPaidAmount { get; set; }

        [JsonProperty]
        internal decimal interestPaidAmount { get; set; }


        [JsonProperty]
        internal string dueDate { get; set; }
    }

    // Modified by BBSR Team on 10th Jan
    public class Activity
    {
        public string payment_date { get; set; }
        public string total_amount { get; set; }
        public string payment_description { get; set; }
        public string due_date { get; set; }
        public string principal_amount { get; set; }
        public string interest_amount { get; set; }
        public string escrow_amount { get; set; }
        public string escrow_balance { get; set; }
        public string principal_balance { get; set; }
        public string feeAmount { get; set; }
        public string lateChargeAmount { get; set; }
        public string miscPaidAmount { get; set; }
        public string suspenseAmount { get; set; }

    }
}
