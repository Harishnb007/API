using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Services.Models.LC_WebApi_Models
{
    public class Authenticate
    {
        [JsonProperty]
        public bool loanPaid { get; set; }

        [JsonProperty]
        public ObjUserInfo objUserInfo { get; set; }


        public string AuthorizationToken { get; set; }
    }


    public class ObjUserInfo
    {
        [JsonProperty]
        public CurrentUserLoan currentUserLoan { get; set; }

        [JsonProperty]
        public string message { get; set; }

        [JsonProperty]
        public int userStatus { get; set; }

        public Users user { get; set; }

    }


    public class CurrentUserLoan
    {
        [JsonProperty]
        public string cellPhone { get; set; }
        [JsonProperty]
        public int clientID { get; set; }
        [JsonProperty]
        public bool disableAmortization { get; set; }
        [JsonProperty]
        public string discAccept { get; set; }
        [JsonProperty]
        public string dueDate { get; set; }
        [JsonProperty]
        public string eStatement { get; set; }
        [JsonProperty]
        public string emailAddress { get; set; }
        [JsonProperty]
        public string estatementPrefDate { get; set; }
        [JsonProperty]
        public string id { get; set; }
        [JsonProperty]
        public string investor { get; set; }
        [JsonProperty]
        public bool isNew { get; set; }
        [JsonProperty]
        public bool isPurged { get; set; }
        [JsonProperty]
        public bool isarm { get; set; }
        [JsonProperty]
        public string loanNo { get; set; }
        [JsonProperty]
        public string loanSource { get; set; }
        [JsonProperty]
        public string loanSourceToString { get; set; }
        [JsonProperty]
        public string notifyEmail { get; set; }
        [JsonProperty]
        public string ssn { get; set; }
        [JsonProperty]
        public long userID { get; set; }
    }


}
