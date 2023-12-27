using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace BaitapLon.Models
{
    public class SanPham
    {
        [Key]
        public int ID { get; set; }
        public string TenSanPham { get; set; }
        public double Gia { get; set; }
        public string MoTa { get; set; }
        public int DanhMucID { get; set; }
        public string? HinhAnh { get; set; }
        public DanhMuc? DanhMuc { get; set; }
        public ICollection<ChiTietDonHang>? chiTietDonHangs { get; set; }
    }
}
