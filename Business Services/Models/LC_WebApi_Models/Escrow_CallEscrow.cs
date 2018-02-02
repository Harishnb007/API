using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Services.Models.LC_WebApi_Models
{
    class Escrow_CallEscrow
    {
        [JsonProperty]
        internal string lastAna { get; set; }

        [JsonProperty]
        internal decimal balEscrow { get; set; }

        [JsonProperty]
        internal decimal balAdvance { get; set; }

        [JsonProperty]
        internal decimal oldEscrowMth { get; set; }

        [JsonProperty]
        internal decimal pmtEscrow { get; set; }

        [JsonProperty]
        internal string payee1 { get; set; }

        [JsonProperty]
        internal string miDisbDueDate { get; set; }

        [JsonProperty]
        internal string guarantyNo { get; set; }

        [JsonProperty]
        internal decimal pmiDisbAmount { get; set; }

        [JsonProperty]
        internal List<EscrowTaxAggregates> escrowTaxAggregates { get; set; }

        [JsonProperty]
        internal List<PropertyInsurancePolicyCollection> propertyInsurancePolicyCollection { get; set; }
    }
    class EscrowTaxAggregates
    {
        [JsonProperty]
        public int billFrequencyType { get; internal set; }

        [JsonProperty]
        internal string taxEscrowItemDescription { get; set; }

        [JsonProperty]
        internal DateTime taxDisbursementDueDate { get; set; }

        [JsonProperty]
        internal decimal taxExpectedDisbursementAmount { get; set; }

        [JsonProperty]
        internal decimal totalExpectedDisbAmt { get; set; }

        [JsonProperty]
        internal string parcelID { get; set; }

        internal string getBillFrequencyTypeText(int billType)
        {
            switch (billType)
            {
                case 0:
                    return "Annually";
                case 1:
                    return "NotListed";
                case 2:
                    return "SemiAnnual";
                case 3:
                    return "TriAnnual";
                case 4:
                    return "Quarterly";
                case 5:
                    return "CouldNotBeListed";
                default:
                    return "Annually";

            }


        }

    }
    class PropertyInsurancePolicyCollection
    {
        public int policyType { get; set; }

        [JsonProperty]
        internal string insuranceCompanyName { get; set; }

        [JsonProperty]
        internal string hazardPolicyNumber { get; set; }

        [JsonProperty]
        internal DateTime policyExpirationDate { get; set; }

        [JsonProperty]
        internal decimal premiumAmount { get; set; }



        internal string getpolicyType(int insuranceType)
        {
            switch (insuranceType)
            {
                case 351:
                    return "Hazard";
                case 352:
                    return "Flood";
                case 353:
                    return "OtherProperty";
                case 354:
                    return "Earthquake";
                case 355:
                    return "Property";
                default:
                    return "";

            }


        }
    }
}
