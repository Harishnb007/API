using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;



namespace Business_Services.Models.DAL.LoancareEntites
{
    public class BankAccounts
    {

        [Key]
        public int bank_acc_db_id { get; set; }

        [ForeignKey("user_db_id")]
        public int user_db_id { get; set; }

        public string routing_number { get; set; }

       public string bank_name { get; set; }

        public string account_number { get; set; }

        public string account_type { get; set; }

        public string legal_name { get; set; }

        public string account_nickname { get; set; }

        public bool is_authorized { get; set; }

        public string created_on { get; set; }

       

    }
}
