using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace BaitapLon.Models
{
    public class User
    {
        [Key] 
        public int ID { get; set; }
        public string TaiKhoan { get; set; }
        public string MatKhau { get; set; }
        public int Admin { get; set; }
        public int KhachHang { get; set; }

    }
}
