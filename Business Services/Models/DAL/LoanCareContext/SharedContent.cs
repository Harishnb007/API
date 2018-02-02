namespace Business_Services.Models.DAL.LoanCareContext
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SharedContent")]
    public partial class SharedContent
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int content_db_id { get; set; }

        [StringLength(30)]
        public string category { get; set; }

        [StringLength(50)]
        public string title { get; set; }

        public string content { get; set; }

        public DateTime? created_on { get; set; }

        public DateTime? updated_on { get; set; }
    }
}
