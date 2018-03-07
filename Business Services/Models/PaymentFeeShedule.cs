using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Services.Models
{
    class PaymentFeeShedule
    {
        [JsonProperty]
        internal DateTime dueDate { get; set; }

        //Defect# 955 : Added by BBSR Team on 6th March 2018
        [JsonProperty]
        internal DateTime aquisitiondDate { get; set; }

        //Defect# 955 : Added by BBSR Team on 6th March 2018
        [JsonProperty]
        internal string AquisitionMessage { get; set; }

        [JsonProperty]
        internal List<PaymentFeeSheduledate> paymentFeesheduledate { get; set; }
    }

    class PaymentFeeSheduledate
    {
        [JsonProperty]
        internal int OverdueStartdays { get; set; }

        [JsonProperty]
        internal int OverdueEnddays { get; set; }

        [JsonProperty]
        internal decimal FeeAmount { get; set; }

      

    }

}
