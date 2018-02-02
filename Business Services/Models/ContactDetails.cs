using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Services.Models
{
    public class ContactDetails
    {
        public string email { get; set; }
        public string phone_primary_number { get; set; }
        public string phone_primary_type { get; set; }
        public string phone_secondary_number { get; set; }
        public string phone_secondary_type { get; set; }
        public string phone_other_1_number { get; set; }
        public string phone_other_1_type { get; set; }
        public string phone_other_2_number { get; set; }
        public string phone_other_2_type { get; set; }
    }
}
