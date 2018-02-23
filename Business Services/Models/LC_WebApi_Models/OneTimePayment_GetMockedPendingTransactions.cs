using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Services.Models.LC_WebApi_Models
{

    public class OneTimePayment_GetMockedPendingTransactions
    {

        [JsonProperty]
        public string schDT { get; set; }

        [JsonProperty]
        public int paymentType { get; set; }
        internal string getPaymentType(int payType)
        {
            switch (payType)
            {

                case 0:
                    return "Payment";
                case 1:
                    return "Principal only";
                case 2:
                    return "Escrow only";
                case 3:
                    return "Fee only";
              
                default:
                    return " ";

            }
        }
    

        [JsonProperty]
        public string accountNumber { get; set; }
        [JsonProperty]
        public decimal amtRecvd { get; set; }
        public string loanNo { get; set; }
        public bool isNew { get; set; }
        public bool skipChildrenRead { get; set; }
        public float principalBalance { get; set; }
        public object bankName { get; set; }
        public string accountName { get; set; }
        public string accountType { get; set; }
        public float addlEscrow { get; set; }
        public float curtailment { get; set; }
        public string dueDate { get; set; }
        public float pmtAmt { get; set; }
        public string totalPaymentAmtDue { get; set; }
        public float totalAmountDue { get; set; }
        public float addlFees { get; set; }
        public float nsfFeesDue { get; set; }
        public float otherFeesDue { get; set; }
        public string noOfDelinquentPaymentsDue { get; set; }
        public string pmts { get; set; }
        public string routTran { get; set; }
        public float totalDraftAmt { get; set; }
        public string loanType { get; set; }
        public bool isAutoDraftSetup { get; set; }
        public DateTime dateTimeStamp { get; set; }
        public string date { get; set; }
        public string time { get; set; }
        public string fullAcctType { get; set; }


        public object acctDraftAmt { get; set; }
        public object act { get; set; }
        public object requestor { get; set; }
        public object assessOnlyDelqFee { get; set; }
        public float delqFee { get; set; }
        public object fee11 { get; set; }
        public object ovr { get; set; }
        public object action { get; set; }
        public bool isOnetimePaymentAlreadySetUp { get; set; }
        public object[] fees { get; set; }
        public Bankaccount[] bankAccounts { get; set; }
        public int clientCode { get; set; }
        public string[] noOfPayments { get; set; }
        public int userRowId { get; set; }
        public float outstandingLateChages { get; set; }
        public float outstandingFeeDue { get; set; }
        public DateTime paymentEffectiveDate { get; set; }

        public string dateCreated { get; set; }
        public object servicerLastProcessingDate { get; set; }
        public bool isStateException { get; set; }
    }
        
    public class Bankaccount
    {
        public int userRowId { get; set; }
        public Contextthread1 contextThread { get; set; }
        public bool isNew { get; set; }
        public bool skipChildrenRead { get; set; }
        public string loanNo { get; set; }
        public string accountNumber { get; set; }
        public string routingNumber { get; set; }
        public string bankName { get; set; }
        public string accountName { get; set; }
        public string accountType { get; set; }
    }

}
