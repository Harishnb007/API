using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Services.Models.LC_WebApi_Models
{
    class MyAccount_GetAccountInfo
    {
        public Msg msg { get; set; }
        public object value { get; set; }
    }

    public class Msg
    {
        public object badCheckStop { get; set; }
        public object coOwners { get; set; }
        public string servicerLastProcessingDate { get; set; }
       
        public bool isNew { get; set; }
        public bool skipChildrenRead { get; set; }
        public string borrower { get; set; }
        public string loanNo { get; set; }
        public string coBorrower { get; set; }
        public string custCity { get; set; }
        public string stateAbbr { get; set; }
        public string custZip { get; set; }
        public string invNo { get; set; }
        public string internetStop { get; set; }
        public string processStop { get; set; }
        public object televoiceStop { get; set; }
        public string loanTerm { get; set; }
        public string telNo { get; set; }
        public string emailAddress { get; set; }
        public string dueDate { get; set; }
        public string custAddress { get; set; }
        public string custCityStZip { get; set; }
        public object owner { get; set; }
        public string borrowerSSN { get; set; }
        public string coBorrowerSSN { get; set; }
        public object ownerSSN { get; set; }
        public object invNo1 { get; set; }
        public object invNo2 { get; set; }
        public object borrowerFN { get; set; }
        public object borrowerLN { get; set; }
        public object borrowerMN { get; set; }
        public object coBorrowerFN { get; set; }
        public object coBorrowerLN { get; set; }
        public object coBorrowerMN { get; set; }
        public int dataProviderLoanType { get; set; }
        public string fullPropertyAddress { get; set; }
        public string pifStopCode { get; set; }
        public object user02PosAlphaNumFld07B { get; set; }
        public object[] owners { get; set; }
        public string alternatePhNumber { get; set; }
        public float priorYearEndPrincipalBalance { get; set; }
    }

 

    public class Disc
    {
        public string stateAbbr { get; set; }
        public Contextthread1 contextThread { get; set; }
        public bool isNew { get; set; }
        public bool skipChildrenRead { get; set; }
        public object licenceName { get; set; }
        public object nmlsNumber { get; set; }
        public object nmlsConsumerAccessWebPage { get; set; }
        public object disclosureMessage { get; set; }
    }

    public class Contextthread1
    {
        public object correlationId { get; set; }
        public int dataProvider { get; set; }
        public object resourceName { get; set; }
        public string sessionId { get; set; }
    }

    public class Lstcoborrower
    {
        public string name { get; set; }
        public object ssn { get; set; }
        public object contextThread { get; set; }
        public bool isNew { get; set; }
        public bool skipChildrenRead { get; set; }
    }



}
