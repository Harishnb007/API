using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Services.Models.LC_WebApi_Models
{
    public class Loan_GetCurrentLoanInfo
    {



            public string balOrigLoan { get; set; }
            public string billMode { get; set; }
            public string cityTaxAmount { get; set; }
            public string countyTaxAmount { get; set; }
            public string dueDate { get; set; }
            public string firstPaymtDate { get; set; }
            public Contextthread contextThread { get; set; }
            public bool isNew { get; set; }
            public bool skipChildrenRead { get; set; }
            public string firstPIPresent { get; set; }
            public string hazPresent { get; set; }
            public string intRate { get; set; }
            public string loanDate { get; set; }
            public string loanType { get; set; }
            public string maturityDate { get; set; }
            public string miAmount { get; set; }
            public string netPresent { get; set; }
            public string overShortAmount { get; set; }
            public string ytdHaz { get; set; }
            public string ytdInt { get; set; }
            public string ytdPrin { get; set; }
            public string ytdTax { get; set; }
            public string loanNo { get; set; }
            public string firstPB { get; set; }
            public string otherTax { get; set; }
            public float reserve { get; set; }
            public float fees { get; set; }
            public float other { get; set; }
            public float otherReceived { get; set; }
            public float balLCDue { get; set; }
            public float ytdPayorFee { get; set; }
            public float escrowBalance { get; set; }
            public object[] paymentOnlyHistory { get; set; }
            public bool disableAmortization { get; set; }
            public bool isarm { get; set; }
            public float mipPaidYTDAmount { get; set; }
            public float escrowInterest { get; set; }
            public float propertyValue { get; set; }
            public object nextArmAdjustment { get; set; }
            public string acquisitionDate { get; set; }
            public object xndeFlag { get; set; }
            public object xnOneTimeDebit { get; set; }
            public string lastTransactionAppliedDate { get; set; }
            public string lastPrinPD { get; set; }
            public string lastIntPD { get; set; }
            public string lastEscrowPD { get; set; }
            public string billingTable { get; set; }
        }

        public class Contextthread
        {
            public object correlationId { get; set; }
            public int dataProvider { get; set; }
            public object resourceName { get; set; }
            public string sessionId { get; set; }
        }

    }

