using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Business_Services.Models.DAL.LoancareEntites
{
  public  class UsersMDb
    {
        
        public int user_db_id { get; set; }
        public bool is_new_mobile_user { get; set; }
        public int mae_steps_completed { get; set; }
        public string Pin { get; set; }
        public string Password { get; set; }
        public string Mobile_Token_Id { get; set; }
        public string User_Id { get; set; }
        public bool Is_Enable_TouchId { get; set; }
        public string Old_Pin { get; set; }
        public string New_Pin { get; set; }
        public DateTime created_on { get; set; }
        public DateTime updated_on { get; set; }
    }
}
