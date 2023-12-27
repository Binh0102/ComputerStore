using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace BaitapLon.Models
{
    public class DonHang
    {
        [Key]
        public int ID { get; set; }
        public double TongTien { get; set; }
        public int UserID { get; set; }
        public string TrangThai { get; set; }
        public string TenKhachHang { get; set; }
        public string SDT { get; set; }
        public string DiaChi { get; set; }
        public User? User { get; set;}
        public ChiTietDonHang? chiTietDonHangs { get; set; }
    }
}
