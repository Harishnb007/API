using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Services.Models.LC_WebApi_Models
{
    class OnetimePayment_GetHolidayList
    {
        [JsonProperty]
        internal List<Holidays> holidays { get; set; }
    }

    class Holidays {

        [JsonProperty]
        internal DateTime h_Date { get; set; }
    }
}
