using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Services.Models.LC_WebApi_Models
{
    public class ValidateSecurityAnswer
    {
        [JsonProperty]
        public string msg { get; set; }

        [JsonProperty]
        public Client client { get; set; }

        [JsonProperty]
        public User user { get; set; }

        [JsonProperty]
        public Userloan userLoan { get; set; }
    }
   
}
