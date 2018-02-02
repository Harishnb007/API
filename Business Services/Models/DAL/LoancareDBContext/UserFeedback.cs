namespace Business_Services.Models.DAL.LoancareDBContext
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("UserFeedback")]
    public partial class UserFeedback
    {
        [Key]
        public int Feedback_db_id { get; set; }

        [Column(TypeName = "date")]
        public DateTime? feedback_date { get; set; }

        [Column(TypeName = "date")]
        public DateTime? created_on { get; set; }

        [StringLength(50)]
        public string created_by { get; set; }

        public int? user_db_id { get; set; }
    }
}
