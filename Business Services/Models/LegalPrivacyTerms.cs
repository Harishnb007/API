using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Services.Models
{
    public class LegalPrivacyTerms
    {
        public string GetLegalPrivacyText { get; set; }
        public int TextVersion { get; set; }
        public string TextType { get; set; }
        public DateTime EffectiveDate { get; set; }
    
    }
}
