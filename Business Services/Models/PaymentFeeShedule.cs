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
