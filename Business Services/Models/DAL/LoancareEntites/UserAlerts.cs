using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;

namespace Business_Services.Models.DAL.LoancareEntites
{
  public class UserAlerts
    {
        [Key]
        public int alert_id { get; set; }

        public string loan_number { get; set; }

        public string message_title { get; set; }

        public string message_date { get; set; }

        public string message_body { get; set; }

        public bool is_deleted { get; set; }

    }
}
