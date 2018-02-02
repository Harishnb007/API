namespace Business_Services.Models.DAL.LoanCareContext
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class BankAccount
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int bank_acc_db_id { get; set; }

        public int bank_id { get; set; }

        public int? user_db_id { get; set; }

        [StringLength(10)]
        public string routing_number { get; set; }

        [StringLength(50)]
        public string bank_name { get; set; }

        [StringLength(50)]
        public string account_number { get; set; }

        [StringLength(50)]
        public string account_type { get; set; }

        [StringLength(50)]
        public string legal_name { get; set; }

        [StringLength(50)]
        public string account_nickname { get; set; }

        public bool? is_authorized { get; set; }

        public DateTime? created_on { get; set; }

        public bool isNew { get; set; }
    }
}
