using Business_Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LoanCare_Mobile_API.Models
{
    public class AuthTokenAndUserDetails
    {
        public AuthTokenAndUserDetails()
        {
            address = new Addresss();
        }
        public string AuthorizationToken { get; set; }
        public DateTime Expires { get; set; }

        public string username { get; set; }
        public int setup_status { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string middle_name { get; set; }
        public string email { get; set; }
        public int id { get; set; }
        public string ssn { get; set; }
        public string password { get; set; }
        public string loanNumber { get; set; }
        public string LoginId { get; set; }
        public string NotifyEmail { get; set; }
        public string discVer { get; set; }
        public bool is_successful { get; set; }
       public string mae_steps_completed { get; set; }
        public string resourcename { get; set; }
    public int ClientId { get; set; }
        public string ClientName { get; set; }
        public string BorrowerName { get; set; }
        public Addresss address { get; set; }
        public bool SecurityQuestionFlag { get; set; }
        public List<LoanSummarys> loans { get; set; }
    }

    public class Addresss
    {
        public bool isForeign { get; set; }
        public string street { get; set; }
        public string city { get; set; }
        public string zipcode { get; set; }
        public string phone { get; set; }
        public string state { get; set; }
        public string country { get; set; }
    }

    public class LoanSummarys
    {
        public string loan_number { get; set; }
        public string property_address { get; set; }
    }
}