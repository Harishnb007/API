namespace Business_Services.Models.DAL.LoancareDBContext
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PaymentTransactionMapping")]
    public partial class PaymentTransactionMapping
    {
        [Key]
        public int mapping_id { get; set; }

        public string source_transaction_code { get; set; }

        public string source_transaction_descr { get; set; }

        public string target_transaction_descr { get; set; }

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
