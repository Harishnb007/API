using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Services.Models
{
   public class ManageNotification
    {

        public bool Payment_Received { get; set; }
        public bool Taxes_Disbursed { get; set; }
        public bool Home_Owner_Insurence_Disbursed { get; set; }
        public bool Flood_Insurence_Disbursed { get; set; }
        public bool Other_Insurence_Disbursed { get; set; }
        public List<GetNotification> GetNotifyList { get; set; }
    }

    public class GetNotification {
        public string userLoanRowId { get; set; }

        public bool isNew { get; set; }
        public bool skipChildrenRead { get; set; }

        public int notificationTypesRowId { get; set; }

        public string loanNumber { get; set; }

        public string roleId { get; set; }
    }
}
