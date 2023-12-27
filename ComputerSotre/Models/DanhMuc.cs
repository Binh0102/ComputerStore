using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace BaitapLon.Models
{
    public class DanhMuc
    {
        [Key] 
        public int ID { get; set; }
        public string TenDanhMuc { get; set; }
        public ICollection<SanPham> SanPhams { get; set; }

    }
}
