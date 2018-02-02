using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;



namespace Business_Services.Models.DAL.LoancareEntites
{
   public class SharedContent
    {
        [Key]
        public int content_db_id { get; set; }

        public string category { get; set; }

        public string title { get; set; }

        public string content { get; set; }

        public DateTime created_on { get; set; }

        public DateTime updated_on { get; set; }
    }
}
