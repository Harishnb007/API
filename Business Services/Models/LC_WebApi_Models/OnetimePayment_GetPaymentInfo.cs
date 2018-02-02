using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Services.Models.LC_WebApi_Models
{
    class OnetimePayment_GetPaymentInfo
    {
        [JsonProperty]
        internal Payment_Inbound payment { get; set; }

        [JsonProperty]
        internal List<Bank_Payment> bankData { get; set; }

    }
    class Payment_Inbound
    {

        [JsonProperty]
        internal decimal amtRecvd { get; set; }

        [JsonProperty]
        internal DateTime schDT { get; set; }

        [JsonProperty]
        internal string pmts { get; set; }

        [JsonProperty]
        internal decimal curtailment { get; set; }

        [JsonProperty]
        internal string routTran { get; set; }

        [JsonProperty]
        internal decimal pmtAmt { get; set; }

        [JsonProperty]
        internal string dueDate { get; set; }


        [JsonProperty]
        internal decimal addlEscrow { get; set; }

        [JsonProperty]

        internal decimal addlFees { get; set; }

        [JsonProperty]

        internal decimal nsfFeesDue { get; set; }
        [JsonProperty]

        internal decimal otherFeesDue { get; set; }
        [JsonProperty]

        internal decimal delqFee { get; set; }

        [JsonProperty]

        internal decimal principalBalance { get; set; }
        [JsonProperty]
        internal string accountNumber { get; set; }
        [JsonProperty]
        internal string routingNumber { get; set; }
        [JsonProperty]
        internal string bankName { get; set; }
        [JsonProperty]
        internal string accountName { get; set; }
        [JsonProperty]
        internal string accountType { get; set; }
        //public int userRowId { get; set; }
        //public Contextthread1 contextThread { get; set; }
        //public bool isNew { get; set; }
        //public bool skipChildrenRead { get; set; }

        [JsonProperty]
        internal string loanType { get; set; }

        [JsonProperty]
        public string userRowId { get; set; }
        public string loanNo { get; set; }

        [JsonProperty]
        internal string paymentType { get; set; }

    }


    class Bank_Payment
    {
        [JsonProperty]
        internal string accountNumber { get; set; }
        [JsonProperty]
        internal string accountName { get; set; }
        [JsonProperty]
        internal string routingNumber { get; set; }
        [JsonProperty]
        internal string bankName { get; set; }
        [JsonProperty]
        internal string accountType { get; set; }
    }

}
