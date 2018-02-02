using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Services.Models
{
    class InsertPayment
    {
        [JsonProperty]
        internal string loanSource { get; set; }

        [JsonProperty]
        internal DateTime dateCreated { get; set; }

        [JsonProperty]
        internal bool isNew { get; set; }

        [JsonProperty]
        internal bool IsAutoDraftSetup { get; set; }

        [JsonProperty]
        internal bool isStateException { get; set; }

        [JsonProperty]
        internal int loanNo { get; set; }

        [JsonProperty]
        internal DateTime schDT { get; set; }

        [JsonProperty]
        internal DateTime intialSchDt { get; set; }

        [JsonProperty]
        internal DateTime PaymentEffectiveDate { get; set; }

        [JsonProperty]
        internal decimal pmtAmt { get; set; }

        [JsonProperty]
        internal int addlFees { get; set; }

        [JsonProperty]
        internal int nSFFeesDue { get; set; }

        [JsonProperty]
        internal bool IsOnetimePaymentAlreadySetUp { get; set; }

        [JsonProperty]
        internal int otherFeesDue { get; set; }

        [JsonProperty]
        internal int delqFee { get; set; }

        [JsonProperty]
        internal decimal principalBalance { get; set; }

        [JsonProperty]
        internal decimal outstandingLateChages { get; set; }

        [JsonProperty]
        internal decimal outstandingFeeDue { get; set; }

        [JsonProperty]
        internal int paymentType { get; set; }

        [JsonProperty]
        internal int userRowId { get; set; }

        [JsonProperty]
        internal DateTime PaymentEffectiveDateFormatted { get; set; }

        [JsonProperty]
        internal DateTime PaymentEffectiveDateFormatted1 { get; set; }

        [JsonProperty]
        internal string PaymentTypeFull { get; set; }

        [JsonProperty]
        internal int isPayment { get; set; }

        [JsonProperty]
        internal int addlEscrow { get; set; }

        [JsonProperty]
        internal DateTime dueDate { get; set; }

        [JsonProperty]
        internal int curtailment { get; set; }

        [JsonProperty]
        internal decimal amtRecvd { get; set; }

        [JsonProperty]
        internal int accountNumber { get; set; }

        [JsonProperty]
        internal int routTran { get; set; }

        [JsonProperty]
        internal string accountName { get; set; }

        [JsonProperty]
        internal string accountType { get; set; }

        [JsonProperty]
        internal string bankName { get; set; }

        [JsonProperty]
        internal int pmts { get; set; }

        [JsonProperty]
        internal string loanType { get; set; }

        [JsonProperty]
        internal int noOfDelinquentPaymentsDue { get; set; }

        [JsonProperty]
        internal string fullAcctType { get; set; }

        [JsonProperty]
        internal string fees { get; set; }
    }
}
