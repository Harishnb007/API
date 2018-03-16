using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Services.Models.LC_WebApi_Models
{
    public class ForgotPassword
    {
        [JsonProperty]
        public string msg { get; set; }

        [JsonProperty]
        public List<secQuesCollection> secQuesCollection { get; set; }

        public User user { get; set; }

    }


    public class secQuesCollection
    {
        [JsonProperty]
        public string confirmSecurityAnswer { get; set; }
        [JsonProperty]
        public bool isNew { get; set; }

        [JsonProperty]
        public string phrases { get; set; }

        [JsonProperty]
        public int questionID { get; set; }

        [JsonProperty]
        public int questionNo { get; set; }

        [JsonProperty]
        public string secretQuestion { get; set; }

        [JsonProperty]
        public string securityAnswer { get; set; }

        [JsonProperty]
        public bool skipChildrenRead { get; set; }

        [JsonProperty]
        public string userFrom { get; set; }

        [JsonProperty]
        public int userID { get; set; }
    }

}
