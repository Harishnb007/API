using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Services.Models.LC_WebApi_Models
{
   public class Pendingloandetails
    {       
            public float? aH_InsurancePmtAmount { get; set; }
            public object defaultInterestAccrued { get; set; }
            public object defaultInterestDue { get; set; }
            public DateTime dueDate { get; set; }
            public float? escrowAdvance { get; set; }
            public float? escrowBalance { get; set; }
            public decimal escrowPaidAmount { get; set; }
            public float? feeAmount { get; set; }
            public string historyTransactionCode { get; set; }
            public decimal interestPaidAmount { get; set; }
            public object lateChargeAmount { get; set; }
            public float? lifeInsurancePmtAmount { get; set; }
            public float? miscPaidAmount { get; set; }
            public decimal principalPmtAmount { get; set; }
            public float? suspenseAmount { get; set; }
            public decimal totalPaymentReceivedAmount { get; set; }
            public string tranCodeDesc { get; set; }
            public DateTime? transactionAppliedDate { get; set; }
            public object unpaidInterest { get; set; }
            public float? unpaidPrincipalBalance { get; set; }        
    }
}
