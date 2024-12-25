namespace De01
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SinhVien")]
    public partial class SinhVien
    {
        [Required]
        [StringLength(30)]
        public string Hotensv { get; set; }

        [Key]
        [StringLength(6)]
        public string MaSV { get; set; }

        public DateTime NgaySinh { get; set; }

        [StringLength(3)]
        public string MaLop { get; set; }
    }
}
