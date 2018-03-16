using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Services.Models.LC_WebApi_Models
{
    class User_GetUserInformation
    {
        public Users user { get; set; }
        public Currentuserloan currentUserLoan { get; set; }
        public int[] roleActions { get; set; }
    }

    public class Users
    {
        public int id { get; set; }
        public string userName { get; set; }
        public bool isNew { get; set; }
        public bool skipChildrenRead { get; set; }
        public string password { get; set; }
        public string ssn { get; set; }
        public string changePassword { get; set; }
        public object status { get; set; }
        public int recordCount { get; set; }
        public bool loadLinkedLoans { get; set; }
        public int linkedLoanStatus { get; set; }
        public Generalrole[] generalRoles { get; set; }
        public string[] userLoansList { get; set; }
        public DateTime passwordUpdatedDate { get; set; }
        public DateTime changePasswordReminderDate { get; set; }
        public DateTime createDate { get; set; }
        public object loginName { get; set; }
        public bool isAdminAuthorizedForLoan { get; set; }
        public int b2bRoleId { get; set; }
        public bool csrAccess { get; set; }
        public string langPref { get; set; }
        public object sftPassword { get; set; }
        public object emailAddress { get; set; }
    }



    public class Generalrole
    {
        public int rowID { get; set; }
        public object contextThread { get; set; }
        public bool isNew { get; set; }
        public bool skipChildrenRead { get; set; }
        public object roleName { get; set; }
        public object roleDescription { get; set; }
        public bool isActive { get; set; }
        public bool isB2BRole { get; set; }
    }

    public class Currentuserloan
    {
        public string loanSourceToString { get; set; }
        public Contextthread1 contextThread { get; set; }
        public bool isNew { get; set; }
        public bool skipChildrenRead { get; set; }
        public int id { get; set; }
        public int userID { get; set; }
        public string loanNo { get; set; }
        public int loanSource { get; set; }
        public string investor { get; set; }
        public int clientID { get; set; }
        public int roleId { get; set; }
        public string discVer { get; set; }
        public string discAccept { get; set; }
        public string emailAddress { get; set; }
        public string notifyEmail { get; set; }
        public string eStatement { get; set; }
        public DateTime dueDate { get; set; }
        public string televoiceStop { get; set; }
        public bool disableAmortization { get; set; }
        public bool isarm { get; set; }
        public string ssn { get; set; }
        public bool isPurged { get; set; }
        public DateTime? estatementPrefDate { get; set; }
    }



}
