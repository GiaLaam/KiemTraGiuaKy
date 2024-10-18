namespace KiemTraGiuaKy.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SanPham")]
    public partial class SanPham
    {
        [Key]
        [StringLength(6)]
        public string MaSanPham { get; set; }

        [Required]
        [StringLength(30)]
        public string TenSanPham { get; set; }

        public DateTime NgayNhap { get; set; }

        [Required]
        [StringLength(2)]
        public string MaLoai { get; set; }

        public virtual LoaiSanPham LoaiSanPham { get; set; }
    }
}
