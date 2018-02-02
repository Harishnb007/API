namespace Business_Services.Models.DAL.LoancareDBContext
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("FeedbackQuestionMaster")]
    public partial class FeedbackQuestionMaster
    {
        [Key]
        public int question_id { get; set; }

        public string question_text { get; set; }

        public int? question_order { get; set; }

        [StringLength(1)]
        public string status { get; set; }

        [Column(TypeName = "date")]
        public DateTime? created_on { get; set; }

        [StringLength(50)]
        public string created_by { get; set; }

        [Column(TypeName = "date")]
        public DateTime? updated_on { get; set; }

        [StringLength(50)]
        public string updated_by { get; set; }
    }
}
