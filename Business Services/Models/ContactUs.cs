using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Services.Models
{
    public class ContactUs
    {
        public string phone { get; set; }
        public string address_helptext { get; set; }
        public string hours_helptext { get; set; }

        public string liveperson_url { get; set; }
        public List<mailing_address> mailing_address { get; set; }
        public List<business_hours> business_hours { get; set; }
        public List<String> email_topics { get; set; }
    }

    public class mailing_address
    {
        public string contact_type { get; set; }
        public string address_line_1 { get; set; }
        public string address_line_2 { get; set; }
        public string address_line_3 { get; set; }
        public string address_line_4 { get; set; }

    }
    public class business_hours
    {
        public string days_text { get; set; }
        public string time_text { get; set; }

    }
}
