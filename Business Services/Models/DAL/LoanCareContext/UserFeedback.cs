namespace Business_Services.Models.DAL.LoanCareContext
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
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int feedback_db_id { get; set; }

        public int? user_db_id { get; set; }

        public string fdbk_question_1 { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? fdbk_rating_1 { get; set; }

        public string fdbk_question_2 { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? fdbk_rating_2 { get; set; }

        public string fdbk_question_3 { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? fdbk_rating_3 { get; set; }

        public string fdbk_question_4 { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? fdbk_rating_4 { get; set; }

        public string fdbk_question_5 { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? fdbk_rating_5 { get; set; }

        public string fdbk_question_6 { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? fdbk_answer_6 { get; set; }

        public DateTime? created_on { get; set; }

        public DateTime? updated_on { get; set; }
    }
}
