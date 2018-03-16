//Added by BBSR Team on 5th March 2018 : Defect # 1218
namespace Business_Services.Models.DAL.LoancareDBContext
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TransDescription")]
    public partial class TransDescription
    {
        [Key]
        public int TransID { get; set; }

        [StringLength(50)]
        public string GroupName { get; set; }

        [StringLength(10)]
        public string TransCode { get; set; }

        [StringLength(200)]
        public string SourceDesc { get; set; }

        [StringLength(200)]
        public string TargetDesc { get; set; }

        [Column(TypeName = "date")]
        public DateTime? UpdatedDate { get; set; }
        
        public int? UpdatedBy { get; set; }
    }
}
//Added by BBSR Team on 5th March 2018