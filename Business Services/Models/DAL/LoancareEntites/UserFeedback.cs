using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

namespace Business_Services.Models.DAL.LoancareEntites
{
   public class UserFeedback
    {

        [Key]
        public int feedback_db_id { get; set; }


        [ForeignKey("user_db_id")]
        public int user_db_id { get; set; }

        public string fdbk_question_1 { get; set; }

        public string fdbk_rating_1 { get; set; }

        public string fdbk_question_2 { get; set; }

        public string fdbk_rating_2 { get; set; }

        public string fdbk_question_3 { get; set; }

        public string fdbk_rating_3 { get; set; }

        public string fdbk_question_4 { get; set; }

        public string fdbk_rating_4 { get; set; }

        public string fdbk_question_5 { get; set; }

        public string fdbk_rating_5 { get; set; }

        public string fdbk_question_6 { get; set; }

        public string fdbk_rating_6 { get; set; }

        public DateTime created_on { get; set; }

        public DateTime updated_on { get; set; }
    }
}
