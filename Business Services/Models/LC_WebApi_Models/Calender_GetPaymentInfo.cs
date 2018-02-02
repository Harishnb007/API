using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Services.Models.LC_WebApi_Models
{
    class Calender_GetPaymentInfo
    {
        [JsonProperty]

        internal Paymentoutbound Payment { get; set; }

       
    }

    class Paymentoutbound
    {

        [JsonProperty]
        internal DateTime dueDate { get; set; }
    }
}
