using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Business_Services.Models.DAL.LoancareEntites
{
  public class SecurityQandA
    {
        [Key]
        public int sec_qanda_db_id { get; set; }

        [ForeignKey("user_db_id")]
        public int user_db_id { get; set; }

        public string question { get; set; }

        public string answer { get; set; }

        public DateTime created_on { get; set; }

        public DateTime updated_on { get; set; }

    }
}
