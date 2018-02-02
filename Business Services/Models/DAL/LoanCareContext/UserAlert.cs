namespace Business_Services.Models.DAL.LoanCareContext
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class UserAlert
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int alert_id { get; set; }

        [StringLength(50)]
        public string loan_number { get; set; }

        public string message_title { get; set; }

        public string message_date { get; set; }

        public string message_body { get; set; }

        public bool? is_deleted { get; set; }
    }
}
