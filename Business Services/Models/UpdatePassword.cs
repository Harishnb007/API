﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Business_Services.Models
{
    public class UpdatePassword
    {
        [JsonProperty]
        public string password { get; set; }

        [JsonProperty]
        public string current_password { get; set; }

        [JsonProperty]
        public string Pin { get; set; }

        public string ConfirmPin { get; set; }
        [JsonProperty]
        public string OldPin { get; set; }

        [JsonProperty]
        public bool is_Success { get; set; }

        [JsonProperty]

        public string Token { get; set; }

        [JsonProperty]

        public string Message { get; set; }
        [JsonProperty]
        public string User_Id { get; set; }
    }


    public class ChangePassword
    {

        public string password { get; set; }
        public string userId { get; set; }
        public string emailaddress { get; set; }
        public string ssn { get; set; }
        public string loanNumber { get; set; }

    }
}
