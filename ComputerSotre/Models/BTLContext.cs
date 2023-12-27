using Microsoft.EntityFrameworkCore;
namespace BaitapLon.Models
{
    public class BTLContext:DbContext
    {
        public BTLContext(DbContextOptions<BTLContext> options) : base(options) { }
        public DbSet<User> Users { get; set; }
        public DbSet<ChiTietDonHang> ChiTietDonHangs { get; set; }
        public DbSet<DanhMuc> DanhMucs { get; set; }
        public DbSet<DonHang> DonHangs { get; set; }
        public DbSet<SanPham> SanPhams { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable("tbUser");
            modelBuilder.Entity<ChiTietDonHang>().ToTable("ChiTietDonHang");
            modelBuilder.Entity<DanhMuc>().ToTable("tbDanhMuc");
            modelBuilder.Entity<DonHang>().ToTable("tbDonHang");
            modelBuilder.Entity<SanPham>().ToTable("tbSanPham");
        }
    }
}
