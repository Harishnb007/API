using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Services.Models.LC_WebApi_Models
{
    class AutoDraft_GetAutoDraft
    {
        [JsonProperty]
        public Autodraftinfo autoDraftInfo { get; set; }

        [JsonProperty]
        public DateTime dueDate { get; set; }

        [JsonProperty]
        public string nextDraftDate { get; set; }
    }

    public class Autodraftinfo
    {
        [JsonProperty]
        public string feePrint { get; set; }

        [JsonProperty]
        public string loanNo { get; set; }

        [JsonProperty]
        public bool isNew { get; set; }

        [JsonProperty]
        public bool skipChildrenRead { get; set; }

        [JsonProperty]
        public string bankName { get; set; }

        [JsonProperty]
        public object accountName { get; set; }

        [JsonProperty]
        public string accountNumber { get; set; }

        [JsonProperty]
        public string fullAcctType { get; set; }

        [JsonProperty]
        public string acctType { get; set; }

        [JsonProperty]
        public object transitNo { get; set; }

        [JsonProperty]
        public string actnCode { get; set; }

        [JsonProperty]
        public decimal? totalDftAmount { get; set; }

        [JsonProperty]
        public object delayDays { get; set; }

        [JsonProperty]
        public object effectDate { get; set; }

        [JsonProperty]
        public string effectDateConfirmations { get; set; }

        [JsonProperty]
        public object addlPrin { get; set; }

        [JsonProperty]
        public object dftFeeAssess { get; set; }

        [JsonProperty]
        public DateTime dateTimeStamp { get; set; }

        [JsonProperty]
        public string date { get; set; }

        [JsonProperty]
        public string time { get; set; }

        [JsonProperty]
        public bool isOnetimePaymentAlreadySetUp { get; set; }

        [JsonProperty]
        public bool isAutoDraftSetup { get; set; }

        [JsonProperty]
        public int clientCode { get; set; }

        [JsonProperty]
        public decimal fee { get; set; }

        [JsonProperty]
        public decimal paymentAmount { get; set; }

        [JsonProperty]
        public decimal totalDraftAmntPrint { get; set; }

        [JsonProperty]
        public string acctTypeDisplayName { get; set; }

        [JsonProperty]
        public object dueDate { get; set; }

        [JsonProperty]
        public object preNoteDate { get; set; }
    }
}
