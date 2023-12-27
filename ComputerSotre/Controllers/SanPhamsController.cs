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
    public class SanPhamsController : BaseController
    {
        private readonly BTLContext _context;

        public SanPhamsController(BTLContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Sreach(string key)
        {
            var sanpham = _context.SanPhams.Where(m => m.TenSanPham.Contains(key)).ToList();
			var DanhMuc = _context.DanhMucs.ToList();
			ViewData["DanhMuc"] = DanhMuc;
			if (IsLogin)
			{
				var user = await _context.Users.FirstOrDefaultAsync(m => m.TaiKhoan == CurrentUser);
				ViewData["User"] = user;
			}
			return View("Index",sanpham);
        }

        // GET: SanPhams
        public async Task<IActionResult> Index()
        {
            var DanhMuc = _context.DanhMucs.ToList();
            ViewData["DanhMuc"] = DanhMuc;
            var bTLContext = _context.SanPhams.Include(s => s.DanhMuc);
            if (IsLogin)
            {
                var user = await _context.Users.FirstOrDefaultAsync(m => m.TaiKhoan == CurrentUser);
                ViewData["User"] = user;
            }
            return View( bTLContext.ToList());
        }
        public async Task<IActionResult> DanhMucDetail(int id)
        {
			var DanhMuc = _context.DanhMucs.ToList();
			ViewData["DanhMuc"] = DanhMuc;
            var SanPham = _context.SanPhams.Where(m => m.DanhMucID == id).ToList();
			if (IsLogin)
			{
				var user = await _context.Users.FirstOrDefaultAsync(m => m.TaiKhoan == CurrentUser);
				ViewData["User"] = user;
			}
			return View("Index",SanPham);
		}

        // GET: SanPhams/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.SanPhams == null)
            {
                return NotFound();
            }
			var DanhMuc = _context.DanhMucs.ToList();
			ViewData["DanhMuc"] = DanhMuc;
			var sanPham = await _context.SanPhams
                .FirstOrDefaultAsync(m => m.ID == id);
            var sanphambydanhmuc = _context.SanPhams.Where(m=>m.DanhMuc.ID==sanPham.DanhMucID).ToList();
            ViewData["Sanpham"] = sanphambydanhmuc;
            if (sanPham == null)
            {
                return NotFound();
            }
			if (IsLogin)
			{
				var user = await _context.Users.FirstOrDefaultAsync(m => m.TaiKhoan == CurrentUser);
				ViewData["User"] = user;
			}
			return View(sanPham);
        }

        // GET: SanPhams/Create
        public IActionResult Create()
        {
            ViewData["DanhMucID"] = new SelectList(_context.DanhMucs, "ID", "ID");
            return View();
        }

        // POST: SanPhams/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,TenSanPham,Gia,MoTa,DanhMucID,Anh")] SanPham sanPham)
        {
            if (ModelState.IsValid)
            {
                _context.Add(sanPham);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DanhMucID"] = new SelectList(_context.DanhMucs, "ID", "ID", sanPham.DanhMucID);
            return View(sanPham);
        }

        // GET: SanPhams/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.SanPhams == null)
            {
                return NotFound();
            }

            var sanPham = await _context.SanPhams.FindAsync(id);
            if (sanPham == null)
            {
                return NotFound();
            }
            ViewData["DanhMucID"] = new SelectList(_context.DanhMucs, "ID", "ID", sanPham.DanhMucID);
            return View(sanPham);
        }

        // POST: SanPhams/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,TenSanPham,Gia,MoTa,DanhMucID,Anh")] SanPham sanPham)
        {
            if (id != sanPham.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(sanPham);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SanPhamExists(sanPham.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["DanhMucID"] = new SelectList(_context.DanhMucs, "ID", "ID", sanPham.DanhMucID);
            return View(sanPham);
        }

        // GET: SanPhams/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.SanPhams == null)
            {
                return NotFound();
            }

            var sanPham = await _context.SanPhams
                .Include(s => s.DanhMuc)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (sanPham == null)
            {
                return NotFound();
            }

            return View(sanPham);
        }

        // POST: SanPhams/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.SanPhams == null)
            {
                return Problem("Entity set 'BTLContext.SanPhams'  is null.");
            }
            var sanPham = await _context.SanPhams.FindAsync(id);
            if (sanPham != null)
            {
                _context.SanPhams.Remove(sanPham);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SanPhamExists(int id)
        {
          return (_context.SanPhams?.Any(e => e.ID == id)).GetValueOrDefault();
        }
    }
}
