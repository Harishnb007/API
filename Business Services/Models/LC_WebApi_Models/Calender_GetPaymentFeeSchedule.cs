using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Services.Models.LC_WebApi_Models
{
    class Calender_GetPaymentFeeSchedule
    {

        [JsonProperty]
        internal List<clientFeeCollection> clientFeeCollection { get; set; }
    }

    class clientFeeCollection
    {
        [JsonProperty]
        internal int daysOverdueStart { get; set; }

        [JsonProperty]
        internal int daysOverdueEnd { get; set; }

        [JsonProperty]
        internal decimal feeAmount { get; set; }

     }
}
