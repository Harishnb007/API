using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Services.Models.LC_WebApi_Models
{
    class personal_getcoborrowercontactInfo
    {

        public class Rootobject
        {
            public Contactinfo contactinfo { get; set; }
        }

        public class Contactinfo
        {
            public Contactinfo1 contactInfo { get; set; }
     
        }

        public class Contactinfo1
        {
            [JsonProperty]
            public string firstName { get; set; }

            [JsonProperty]
            public string lastOrOrganizationName { get; set; }

        }  
    }
}
