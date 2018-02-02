namespace Business_Services.Models.DAL.LoanCareContext
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class UserSubscription
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int subscription_db_id { get; set; }

        [StringLength(50)]
        public string loan_number { get; set; }

        public bool? is_pay_received_push { get; set; }

        public bool? is_pay_received_text { get; set; }

        public bool? is_pay_remainder_push { get; set; }

        public bool? is_pay_remainder_text { get; set; }

        public bool? is_tax_disb_push { get; set; }

        public bool? is_tax_disb_text { get; set; }

        public bool? is_homeinsurance_disb_push { get; set; }

        public bool? is_homeinsurance_disb_text { get; set; }

        public bool? is_floodinsurance_disb_push { get; set; }

        public bool? is_floodinsurance_disb_text { get; set; }

        public bool? is_otherinsurance_disb_push { get; set; }

        public bool? is_otherinsurance_disb_text { get; set; }

        public bool? is_estatement_available_push { get; set; }

        public bool? is_estatement_available_text { get; set; }

        public bool? is_user_authorized { get; set; }

        public DateTime? created_on { get; set; }

        public DateTime? updated_on { get; set; }
    }
}
