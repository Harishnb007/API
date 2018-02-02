using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Services.Models
{
    //Added by BBSR_Team on 23rd Dec 2017
    public class SecurityQuestion
    {
        public List<SecurityQuestionSummary> questions { get; set; }
    }

    public class SecurityQuestionSummary
    {
        //Modified by BBSR_Team on 12thJan2018
        public string questionid { get; set; }
        public string userid { get; set; }
        //Modified by BBSR_Team on 12thJan2018
        public string question { get; set; }
        public string answer { get; set; }
    }
}