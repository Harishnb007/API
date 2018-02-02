using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Services.Models
{
   public class PushNotificationUser
    {
        public bool PushNotification { get; set; }
        public bool PaymentAlerts { get; set; }
        public string Login_Id { get; set; }
    }
}
