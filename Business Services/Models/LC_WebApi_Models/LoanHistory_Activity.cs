using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Services.Models.LC_WebApi_Models
{
    public class LoanHistory_Activity
    {
    
        public string balEscrow { get; set; }
        public string balLCDue { get; set; }
        public string firstPB { get; set; }
        public string loanNo { get; set; }
        public Contextthread contextThread { get; set; }
        public bool isNew { get; set; }
        public bool skipChildrenRead { get; set; }
        public string defaultOtherFeesDueAmount { get; set; }
        public Fullmortagehistory[] fullMortageHistory { get; set; }
        public object disbursementDataCollection { get; set; }
        public float reserveBalance { get; set; }
        public float undisbursed { get; set; }
        public float totalPayment { get; set; }
        public float unpaidInterest { get; set; }
        public float unpaidDefaultInterest { get; set; }
    }



    public class Fullmortagehistory
    {
        public float aH_InsurancePmtAmount { get; set; }
        public object defaultInterestAccrued { get; set; }
        public object defaultInterestDue { get; set; }
        public DateTime dueDate { get; set; }
        public float escrowAdvance { get; set; }
        public float escrowBalance { get; set; }
        public float escrowPaidAmount { get; set; }
        public float? feeAmount { get; set; }
        public string historyTransactionCode { get; set; }
        public string interestPaidAmount { get; set; }
        public float? lateChargeAmount { get; set; }
        public float lifeInsurancePmtAmount { get; set; }
        public float miscPaidAmount { get; set; }
        public float principalPmtAmount { get; set; }
        public float suspenseAmount { get; set; }
        public float totalPaymentReceivedAmount { get; set; }
        public string tranCodeDesc { get; set; }
        public DateTime transactionAppliedDate { get; set; }
        public object unpaidInterest { get; set; }
        public float unpaidPrincipalBalance { get; set; }
        public Contextthread1 contextThread { get; set; }
        public bool isNew { get; set; }
        public bool skipChildrenRead { get; set; }
    }

   

}
