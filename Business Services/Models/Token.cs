using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Services.Models
{
    public class Token
    {
        public DateTime IssuedOn { get; set; }
        public string UserId { get; set; }
        public DateTime ExpiresOn { get; set; }
        public string Lcauth { get; set; }
        public string Password { get; set; }
        public string UserName { get; set; }
        public int ClientId { get; set; }
        public string resourcename { get; set; }
        public string log { get; set; }
    }
}
