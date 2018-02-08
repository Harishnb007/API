using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Services.Models.LC_WebApi_Models
{
    public class secQuesCollectionforgotuser
    {
        public SequrityQuestionUser user { get; set; }
        public SequrityQuestionUserLoan userLoan { get; set; }
       public List<SecurityQuestionForgotUser> secQuesCollection { get; set; }
    }
    public class SequrityQuestionUser {

        public int id { get; set; }
        public string ssn { get; set; }
    }

    public class SequrityQuestionUserLoan {

        public int id { get; set; }

        public string loanNo { get; set; }

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
