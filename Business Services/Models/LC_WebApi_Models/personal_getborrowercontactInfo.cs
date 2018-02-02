using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Services.Models.LC_WebApi_Models
{
    class personal_getborrowercontactInfo
    {
        [JsonProperty]
        public Contactinfo contactinfo { get; set; }
    }

    public class Contactinfo
    {
        public Contactinfo1 contactInfo { get; set; }
        public object errorCode { get; set; }
        public object errorMessage { get; set; }
        public bool isSuccess { get; set; }
    }

    public class Contactinfo1
    {
        public string borrower { get; set; }
        public string clientId { get; set; }
        public string coBorrower { get; set; }
        public object faxNumber { get; set; }
        public string firstName { get; set; }
        public bool isInternationalAddress { get; set; }
        public string lastOrOrganizationName { get; set; }
        public string loanNumber { get; set; }
        public string mailingAddressCityName { get; set; }
        public object mailingAddressCountry { get; set; }
        public object mailingAddressLine1 { get; set; }
        public object mailingAddressLine2 { get; set; }
        public string mailingAddressPostalCode { get; set; }
        public string mailingAddressStateAbbreviation { get; set; }
        public string mailingAddressStreet { get; set; }
        public object middleName { get; set; }
        public Othertelecomnumber[] otherTelecomNumbers { get; set; }
        public Primarytelecomnumber primaryTelecomNumber { get; set; }
        public string propertyAddressCityStateZip { get; set; }
        public string propertyAddressStreet { get; set; }
        public Secondarytelecomnumber secondaryTelecomNumber { get; set; }
    }

    public class Primarytelecomnumber
    {
        public object bestTime { get; set; }
        public string consentRevokeIndicatorCode { get; set; }
        public string consentRevokeIndicatorDate { get; set; }
        public string consentRevokeSourceCode { get; set; }
        public string phoneNumber { get; set; }
        public string priorityCode { get; set; }
        public int sequenceNumber { get; set; }
        public string sourceCode { get; set; }
        public string type { get; set; }
    }

    public class Secondarytelecomnumber
    {
        public object bestTime { get; set; }
        public object consentRevokeIndicatorCode { get; set; }
        public object consentRevokeIndicatorDate { get; set; }
        public object consentRevokeSourceCode { get; set; }
        public string phoneNumber { get; set; }
        public string priorityCode { get; set; }
        public int sequenceNumber { get; set; }
        public string sourceCode { get; set; }
        public string type { get; set; }
    }

    public class Othertelecomnumber
    {
        public object bestTime { get; set; }
        public object consentRevokeIndicatorCode { get; set; }
        public object consentRevokeIndicatorDate { get; set; }
        public object consentRevokeSourceCode { get; set; }
        public string phoneNumber { get; set; }
        public object priorityCode { get; set; }
        public int sequenceNumber { get; set; }
        public object sourceCode { get; set; }
        public string type { get; set; }
    }
}