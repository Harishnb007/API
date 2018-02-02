using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
namespace Business_Services.Models
{
  public class ConfirmRegistration
    {
        [JsonProperty]
        public string userid { get; set; }
        [JsonProperty]
        public string ssn { get; set; }
        [JsonProperty]
        internal string discVer { get; set; }

        [JsonProperty]
        internal string content { get; set; }
        [JsonProperty]
        internal string discAccept { get; set; }
    }
}
