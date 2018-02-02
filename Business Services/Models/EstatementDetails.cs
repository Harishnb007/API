using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Services.Models
{
  public class EstatementDetails
    {
        public List<EsatementDateurl> estatement { get; set; }
    }
    public class EsatementDateurl
    {

        public string statement_date { get; set; }

        public string statement_url { get; set; }

    }
}
