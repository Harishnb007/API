using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Services.Models
{
   public class LoanContactDetail
    {
        [JsonProperty]
        internal string Username { get; set; }

        [JsonProperty]
        internal string email { get; set; }
        
        [JsonProperty]
        internal string phone_primary_number { get; set; }

        [JsonProperty]
        internal string phone_primary_type { get; set; }

        [JsonProperty]
        internal string phone_secondary_number { get; set; }

        [JsonProperty]
        internal string phone_secondary_type { get; set; }

        [JsonProperty]
        internal string phone_other_1_number { get; set; }

        [JsonProperty]
        internal string phone_other_1_type { get; set; }

        [JsonProperty]
        internal string phone_other_2_number { get; set; }

        [JsonProperty]
        internal string phone_other_2_type { get; set; }

        [JsonProperty]
        internal string phone_other_3_number { get; set; }

        [JsonProperty]
        internal string phone_other_3_type { get; set; }

        [JsonProperty]
        internal string zipcode { get; set; }

        [JsonProperty]
        internal string state { get; set; }

        [JsonProperty]
        internal string city { get; set; }

        [JsonProperty]
        internal string Address1 { get; set; }

        [JsonProperty]
        internal string Address2 { get; set; }

        [JsonProperty]
        internal string street { get; set; }

        [JsonProperty]
        internal string sequenceNumber1 { get; set; }

        [JsonProperty]
        internal string sequenceNumber2 { get; set; }

        [JsonProperty]
        internal string sequenceNumber3 { get; set; }

        [JsonProperty]
        internal string LoanNumber { get; set; }

        [JsonProperty]
        internal bool is_enrolled { get; set; }

        [JsonProperty]
        internal bool is_Foreign { get; set; }

        [JsonProperty]
        internal string Token { get; set; }

        //Added By BBSR Team on 6th Jan 2018
        [JsonProperty]
        internal string consentRevokeIndicatorCode_primary { get; set; }

        //Added By BBSR Team on 6th Jan 2018
        [JsonProperty]
        internal string consentRevokeIndicatorCode_secondary { get; set; }

        //Added By BBSR Team on 6th Jan 2018
        [JsonProperty]
        internal string consentRevokeIndicatorCode_other1 { get; set; }

        //Added By BBSR Team on 6th Jan 2018
        [JsonProperty]
        internal string consentRevokeIndicatorCode_other2 { get; set; }

        //Added By BBSR Team on 6th Jan 2018
        [JsonProperty]
        internal string consentRevokeIndicatorCode_other3 { get; set; }
        [JsonProperty]
        internal string country { get; set; }
    }
}
