namespace Business_Services.Models.DAL.LoancareDBContext
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class LegalPrivacyTerm
    {
        [Key]
        public int LPT_ID { get; set; }

        [StringLength(20)]
        public string Type { get; set; }

        public string Formatted_Text { get; set; }

        public int? version { get; set; }

        [Column(TypeName = "date")]
        public DateTime? EffectiveFromDate { get; set; }
    }
}
