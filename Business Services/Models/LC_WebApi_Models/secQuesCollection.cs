using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Services.Models.LC_WebApi_Models
{
    public class secQuesCollectionforgotuser
    {
        public string loanNo { get; set; }
        public string ssn { get; set; }
        public List<string> secQuesCollection { get; set; }
    }

    public class SecurityQuestionForgotUser
    {
        public int userID { get; set; }

        public Boolean isNew { get; set; }

        public Boolean skipChildrenRead { get; set; }

        public int questionID { get; set; }

        public string secretQuestion { get; set; }

        public string phrases { get; set; }

        public string securityAnswer { get; set; }

        public string confirmSecurityAnswer { get; set; }

        public string userFrom { get; set; }

        public int questionNo { get; set; }
    }
}
