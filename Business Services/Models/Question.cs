using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Services.Models
{
    //Added by BBSR_Team on 25th Dec 2017
    public class Question
    {
        public List<SecurityQuestionSummary> secquestions { get; set; }               
    }

    public class QuestionSummary
    {
        public int userID { get; set; }
        public int userFrom { get; set; }
        public int questionID { get; set; }
        public string secretQuestion { get; set; }
        public string securityAnswer { get; set; }
    }
}
