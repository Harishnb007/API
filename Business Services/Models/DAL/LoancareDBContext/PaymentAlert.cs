namespace Business_Services.Models.DAL.LoancareDBContext
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PaymentAlert")]
    public partial class PaymentAlert
    {
        [Key]
        public int pmt_transaction_id { get; set; }

        [StringLength(20)]
        public string loan_number { get; set; }

        public int? pmt_transaction_code { get; set; }

        public double? pmt_total_amount { get; set; }

        [Column(TypeName = "date")]
        public DateTime? pmt_trasaction_date { get; set; }

        public int? pmt_trasaction_sk { get; set; }

        public string alert_title { get; set; }

        [Column(TypeName = "date")]
        public DateTime? alert_date { get; set; }

        [StringLength(1)]
        public string alert_read_status { get; set; }
    }
}
