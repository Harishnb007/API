namespace Business_Services.Models.DAL.LoanCareContext
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Privacy")]
    public partial class Privacy
    {
        [Key]
        public int P_ID { get; set; }

        public string heading { get; set; }

        public string detail { get; set; }

        [StringLength(10)]
        public string Account_ID { get; set; }
    }
}
