namespace Business_Services.Models.DAL.LoancareDBContext
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("AlertTemplate")]
    public partial class AlertTemplate
    {
        [Key]
        public int template_id { get; set; }

        public int pmt_transaction_code { get; set; }

        public string title_template { get; set; }

        public string alert_detail_template { get; set; }

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
