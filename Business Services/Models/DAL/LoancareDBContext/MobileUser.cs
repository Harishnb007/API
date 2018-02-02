namespace Business_Services.Models.DAL.LoancareDBContext
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("MobileUser")]
    public partial class MobileUser
    {
        [Key]
        public int user_db_id { get; set; }

        [StringLength(20)]
        public string mae_steps_completed { get; set; }

        [StringLength(20)]
        public string pin { get; set; }

        [Column(TypeName = "date")]
        public DateTime? created_on { get; set; }

        [Column(TypeName = "date")]
        public DateTime? updated_on { get; set; }

        [StringLength(20)]
        public string User_Id { get; set; }

        [StringLength(20)]
        public string Mobile_Token_Id { get; set; }

        public bool? Is_New_MobileUser { get; set; }

        public int? Legal_version { get; set; }

        public int? Terms_version { get; set; }

        public int? Privacy_version { get; set; }
    }
}
