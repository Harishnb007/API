using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Business_Services.Models.Helpers
{
    internal class ResponseWithToken
    {
        internal HttpResponseMessage message { get; set; }
        internal string tokenValue { get; set; }
    }
}
