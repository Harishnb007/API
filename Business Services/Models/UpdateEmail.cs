using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Services.Models
{
  public  class UpdateEmail
    {
        [JsonProperty]
        internal string email { get; set; }

        [JsonProperty]
        internal string notifyEmail { get; set; }

        [JsonProperty]
        internal string loanNo { get; set; }

        [JsonProperty]
        internal string discVer { get; set; }

        [JsonProperty]
        internal string discAccept { get; set; }

        [JsonProperty]
        internal string Token { get; set; }

        [JsonProperty]
        internal bool issuccess { get; set; }

        [JsonProperty]
        internal string Message { get; set; }
    }
}
