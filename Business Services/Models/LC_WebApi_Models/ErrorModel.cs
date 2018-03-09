using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Services.Models.LC_WebApi_Models
{
    public class ErrorModel
    {
        [JsonProperty]
        public int errorID { get; set; }

        [JsonProperty]
        public string errorMessage { get; set; }

        [JsonProperty]
        public string message { get; set; }

        [JsonProperty]
        public string msg { get; set; }
    }
}
