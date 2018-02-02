using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LoanCare_Mobile_API.Models
{
    public class UserCred
    {
        public string username { get; set; }
        public string password { get; set; }
        public bool Is_New_MobileUser { get; set; }
    }
}