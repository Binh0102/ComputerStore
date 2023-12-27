using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BaitapLon.Models;

namespace BaitapLon.Controllers
{
    public class DonHangsController : BaseController
    {
        private readonly BTLContext _context;

        public DonHangsController(BTLContext context)
        {
            _context = context;
        }

        // GET: DonHangs
        public async Task<IActionResult> Index()
        {
			var DanhMuc = _context.DanhMucs.ToList();
			ViewData["DanhMuc"] = DanhMuc;
			if (IsLogin)
			{
				var user = await _context.Users.FirstOrDefaultAsync(m => m.TaiKhoan == CurrentUser);
				ViewData["User"] = user;
			}
            double sum = 0;
            foreach(var cart in ListCart)
            {
                sum = sum + cart.SanPham.Gia * cart.SoLuong;
            }
            ViewData["TongTien"] = sum;
            return View();
		}

        // GET: DonHangs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.DonHangs == null)
            {
                return NotFound();
            }

            var donHang = await _context.DonHangs
                .FirstOrDefaultAsync(m => m.ID == id);
            if (donHang == null)
            {
                return NotFound();
            }

            return View(donHang);
        }

        // GET: DonHangs/Create

        public async Task<IActionResult> ThanhToan(string TenKhachHang, string SDT, string DiaChi, double TongTien)
        {
            DonHang donhang = new DonHang();
            donhang.TenKhachHang = TenKhachHang;
            donhang.SDT = SDT;;
            donhang.DiaChi = DiaChi;
            donhang.TongTien = TongTien;
            donhang.TrangThai = "dang van chuyen";
            var user = _context.Users.FirstOrDefault(m => m.TaiKhoan == CurrentUser);
            donhang.UserID = user.ID;
            _context.Add(donhang);
            await _context.SaveChangesAsync();
            List<Cart> listcart = ListCart;
            foreach(var cart in listcart)
            {
                string sql = "insert into ChiTietDonHang (DonHangID,SanPhamID,SoLuong) values (" + donhang.ID + "," + cart.SanPham.ID + "," + cart.SoLuong + ")";
                _context.Database.ExecuteSqlRaw(sql);
            }
            listcart.Clear();
            ListCart = listcart;
            return RedirectToAction("Index", "SanPhams");
        }

        public async Task<IActionResult> QLDonHang()
        {
            var user = _context.Users.FirstOrDefault(m => m.TaiKhoan == CurrentUser);
            var bTLContext = _context.DonHangs.Where(s => s.UserID == user.ID);
            var DanhMuc = _context.DanhMucs.ToList();
            ViewData["DanhMuc"] = DanhMuc;
            if (IsLogin)
            {
                ViewData["User"] = user;
            }
            return View(bTLContext.ToList());
        }

        public async Task<IActionResult> ChiTietDonHang(int id)
        {
            var ctDonHang = _context.ChiTietDonHangs
                .Include(m=> m.SanPham)
                .Where(m => m.DonHangID == id).ToList();
            var DanhMuc = _context.DanhMucs.ToList();
            ViewData["DanhMuc"] = DanhMuc;
            if (IsLogin)
            {
                var user = await _context.Users.FirstOrDefaultAsync(m => m.TaiKhoan == CurrentUser);
                ViewData["User"] = user;
            }
            return View(ctDonHang);
        }
        public async Task<IActionResult> QLDonHangAD()
        {
			var bTLContext = _context.DonHangs.ToList();
			var DanhMuc = _context.DanhMucs.ToList();
			ViewData["DanhMuc"] = DanhMuc;
			if (IsLogin)
			{
				var user = _context.Users.FirstOrDefault(m => m.TaiKhoan == CurrentUser);
				ViewData["User"] = user;
			}
			return View(bTLContext);
		}
        
        public async Task<IActionResult> Edit(int id)
        {
            var donHang = _context.DonHangs.FirstOrDefault(m => m.ID == id);
            donHang.TrangThai = "Da giao hang";
            _context.Update(donHang);
            await _context.SaveChangesAsync();
            return RedirectToAction("QLDonHangAD");
        }

        // GET: DonHangs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.DonHangs == null)
            {
                return NotFound();
            }

            var donHang = await _context.DonHangs
                .FirstOrDefaultAsync(m => m.ID == id);
            if (donHang == null)
            {
                return NotFound();
            }

            return View(donHang);
        }

        // POST: DonHangs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.DonHangs == null)
            {
                return Problem("Entity set 'BTLContext.DonHangs'  is null.");
            }
            var donHang = await _context.DonHangs.FindAsync(id);
            if (donHang != null)
            {
                _context.DonHangs.Remove(donHang);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DonHangExists(int id)
        {
          return (_context.DonHangs?.Any(e => e.ID == id)).GetValueOrDefault();
        }
    }
}
