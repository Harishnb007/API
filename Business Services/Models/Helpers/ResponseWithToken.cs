using Business_Services.Models.LC_WebApi_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Business_Services.Models.Helpers
{
    public class ResponseWithToken
    {
        public HttpResponseMessage message { get; set; }
        public string tokenValue { get; set; }
        public string errorMessage { get; set; }
        public int errorId { get; set; }
        public Authenticate authenticateResult { get; set; }

        public string changePassword { get; set; }
    }
}
