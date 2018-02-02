using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Services.Models.LC_WebApi_Models
{
    class Getdetails_estatement
    {
        public Datum[] data { get; set; }
        public bool notify { get; set; }
    }

    public class Datum
    {
        public string description { get; set; }
        public string key { get; set; }
        public string statementDate { get; set; }
    }

}

