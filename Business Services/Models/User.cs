using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Services.Models
{
    public class User
    {

        public User()
        {
            addresss = new Address();
        }
        public string username { get; set; }
        public int setup_status { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string middle_name { get; set; }
        public string email { get; set; }
        public int id { get; set; }
        public string ssn { get; set; }
        public string password { get; set; }
        public string LoginId { get; set; }
        public string loanNumber { get; set; }
        public string NotifyEmail { get; set; }
        public string discVer { get; set; }
        public string ClientName { get; set; }
        //Added by BBSR Team on 16th Jan 2018
        public string discAccept { get; set; }
        public string BorrowerName { get; set; }
        public int ClientId { get; set; }
        public bool is_successful { get; set; }
        public string Token { get; set; }
        public Address addresss { get; set; }
        public string status { get; set; }
        public string noOfAttempts { get; set; }

        public List<LoanSummary> loanss { get; set; }
    }

    public class Address
    {
        public bool isForeign { get; set; }
        public string street { get; set; }
        public string city { get; set; }
        public string zipcode { get; set; }
        public string phone { get; set; }
        public string state { get; set; }
        public string country { get; set; }
    }

    public class LoanSummary
    {
        public string loan_number { get; set; }
        public string property_address { get; set; }
    }
}
