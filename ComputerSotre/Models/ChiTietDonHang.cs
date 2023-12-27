using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace BaitapLon.Models
{
    public class ChiTietDonHang
    {
        [Key]
        public int ID { get; set; }
        public int DonHangID { get; set; }
        public int SanPhamID { get; set; }
        public int SoLuong { get; set; }
        public DonHang? DonHang { get; set; }
        public SanPham? SanPham { get; set; }

    }
}
