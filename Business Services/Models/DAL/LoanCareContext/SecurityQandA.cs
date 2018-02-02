namespace Business_Services.Models.DAL.LoanCareContext
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SecurityQandA")]
    public partial class SecurityQandA
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int sec_qanda_db_id { get; set; }

        public int? user_db_id { get; set; }

        public string question { get; set; }

        public string answer { get; set; }

        public DateTime? created_on { get; set; }

        public DateTime? updated_on { get; set; }
    }
}
