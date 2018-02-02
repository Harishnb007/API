using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Services.Models.LC_WebApi_Models
{
    class Auth_GetToken
    {
        [JsonProperty]
        public string formToken { get; set; }

        [JsonProperty]
        public string cookieToken { get; set; }

    }
}
