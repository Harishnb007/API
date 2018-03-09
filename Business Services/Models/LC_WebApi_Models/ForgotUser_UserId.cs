using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Services.Models.LC_WebApi_Models
{
    
    public class ForgotUser_UserId
{
    public string msg { get; set; }
    public User user { get; set; }
    public Userloan userLoan { get; set; }
    public Client client { get; set; }
}

public class User
{
    public int id { get; set; }
    public string userName { get; set; }
    
    public bool isNew { get; set; }
    public bool skipChildrenRead { get; set; }
    public string password { get; set; }
    public string ssn { get; set; }
    public string changePassword { get; set; }
    public string status { get; set; }
    public int recordCount { get; set; }
    public bool loadLinkedLoans { get; set; }
    public int linkedLoanStatus { get; set; }
    public object[] generalRoles { get; set; }
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
    public string emailAddress { get; set; }
    public object recoveryPin { get; set; }
    public object contactType { get; set; }
}



public class Userloan
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
    public object eStatement { get; set; }
    public DateTime dueDate { get; set; }
    public string televoiceStop { get; set; }
    public bool disableAmortization { get; set; }
    public bool isarm { get; set; }
    public string ssn { get; set; }
    public bool isPurged { get; set; }
    public DateTime estatementPrefDate { get; set; }
}


public class Client
{
    public object chatLogo { get; set; }
    public object invCSV { get; set; }
    public object monFriEnd { get; set; }
    public object monFriStart { get; set; }
    public object satEnd { get; set; }
    public object satStart { get; set; }
    public object sunEnd { get; set; }
    public object sunStart { get; set; }
    public object zoneCSV { get; set; }
    public Contextthread2 contextThread { get; set; }
    public bool isNew { get; set; }
    public bool skipChildrenRead { get; set; }
    public int clientCode { get; set; }
    public string clientName { get; set; }
    public string headerLogo1 { get; set; }
    public object privacyURL { get; set; }
    public string privacyText { get; set; }
    public string privateLabelURL { get; set; }
    public string copyrightText { get; set; }
    public string clientDirectory { get; set; }
    public string clientPhone { get; set; }
    public string css { get; set; }
    public string cssMobile { get; set; }
    public Advertisement[] advertisements { get; set; }
    public int hasChatFeature { get; set; }
    public string clientNameCaption { get; set; }
    public string livePersonURL { get; set; }
    public object customerSupportEmail { get; set; }
    public object clientStatus { get; set; }
    public object clientSource { get; set; }
    public object clientPLSId { get; set; }
    public object yearEndName { get; set; }
    public object receivesLFN { get; set; }
    public object comments { get; set; }
    public object[] emails { get; set; }
    public object[] phones { get; set; }
    public Zones zones { get; set; }
    public object[] addresses { get; set; }
    public Invcodes invCodes { get; set; }
    public object clientURL { get; set; }
    public object sowLevel { get; set; }
    public string welcomeText { get; set; }
    public Welcometext[] welcomeTexts { get; set; }
}

public class Contextthread2
{
    public object correlationId { get; set; }
    public int dataProvider { get; set; }
    public object resourceName { get; set; }
    public object sessionId { get; set; }
}

public class Zones
{
    public Clientzonecollection[] clientZoneCollection { get; set; }
    public Contextthread3 contextThread { get; set; }
    public bool isNew { get; set; }
    public bool skipChildrenRead { get; set; }
    public int clientCode { get; set; }
}

public class Contextthread3
{
    public object correlationId { get; set; }
    public int dataProvider { get; set; }
    public object resourceName { get; set; }
    public object sessionId { get; set; }
}

public class Clientzonecollection
{
    public int rowId { get; set; }
    public string zone { get; set; }
    public Contextthread4 contextThread { get; set; }
    public bool isNew { get; set; }
    public bool skipChildrenRead { get; set; }
    public int clientCode { get; set; }
    public string modifiedBy { get; set; }
    public string modificationDate { get; set; }
    public object commaSeperatedList { get; set; }
}

public class Contextthread4
{
    public object correlationId { get; set; }
    public int dataProvider { get; set; }
    public object resourceName { get; set; }
    public object sessionId { get; set; }
}

public class Invcodes
{
    public Invcodecollection[] invCodeCollection { get; set; }
    public Contextthread5 contextThread { get; set; }
    public bool isNew { get; set; }
    public bool skipChildrenRead { get; set; }
    public int clientCode { get; set; }
}

public class Contextthread5
{
    public object correlationId { get; set; }
    public int dataProvider { get; set; }
    public object resourceName { get; set; }
    public object sessionId { get; set; }
}

public class Invcodecollection
{
    public string inv { get; set; }
    public Contextthread6 contextThread { get; set; }
    public bool isNew { get; set; }
    public bool skipChildrenRead { get; set; }
    public int clientCode { get; set; }
    public object lastModBy { get; set; }
    public object commaSeperatedList { get; set; }
}

public class Contextthread6
{
    public object correlationId { get; set; }
    public int dataProvider { get; set; }
    public object resourceName { get; set; }
    public object sessionId { get; set; }
}

public class Advertisement
{
    public string advImage { get; set; }
    public string advUrl { get; set; }
    public int advWeight { get; set; }
    public int clientCode { get; set; }
    public int rowId { get; set; }
    public Contextthread7 contextThread { get; set; }
    public bool isNew { get; set; }
    public bool skipChildrenRead { get; set; }
}

public class Contextthread7
{
    public object correlationId { get; set; }
    public int dataProvider { get; set; }
    public object resourceName { get; set; }
    public object sessionId { get; set; }
}

public class Welcometext
{
    public int clientCode { get; set; }
    public string locale { get; set; }
    public string text { get; set; }
    public Contextthread8 contextThread { get; set; }
    public bool isNew { get; set; }
    public bool skipChildrenRead { get; set; }
    public string langauge { get; set; }
}

public class Contextthread8
{
    public object correlationId { get; set; }
    public int dataProvider { get; set; }
    public object resourceName { get; set; }
    public object sessionId { get; set; }
}

    
}
