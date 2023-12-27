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
    public class QLSanPhamController : BaseController
    {
        private readonly BTLContext _context;

        public QLSanPhamController(BTLContext context)
        {
            _context = context;
        }

        // GET: QLSanPham
        public async Task<IActionResult> Index()
        {
            var bTLContext = _context.SanPhams.Include(s => s.DanhMuc );
            
            var DanhMuc = _context.DanhMucs.ToList();
            ViewData["DanhMuc"] = DanhMuc;
            if (IsLogin)
            {
                var user = await _context.Users.FirstOrDefaultAsync(m => m.TaiKhoan == CurrentUser);
                ViewData["User"] = user;
            }
            return View(bTLContext.ToList());
        }

        // GET: QLSanPham/Details/5
        public async Task<IActionResult> Details(int? id)
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
        private string UploadAvatar(int ID, IFormFile file)
        {

            string path = Path.GetFullPath("./wwwroot/Data");
            Directory.CreateDirectory(path + "/" + ID);

            using var stream = new FileStream(string.Format(@"{0}/{1}/{2}", path, ID, file.FileName), FileMode.Create);
            file.CopyTo(stream);
            return ID + "/" + file.FileName;
        }
        private bool DeleteAvatar(int ID)
        {
            try
            {
                string path = Path.GetFullPath("./wwwroot/Data/" + ID);
                Directory.Delete(path, true);
                return true;
            }
            catch
            {
                return false;
            }
        }
        // GET: QLSanPham/Create
        public IActionResult Create()
        {
            ViewData["DanhMucID"] = new SelectList(_context.DanhMucs, "ID", "TenDanhMuc");
            return View();
        }

        // POST: QLSanPham/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,TenSanPham,Gia,MoTa,DanhMucID,HinhAnh")] SanPham sanPham, IFormFile file)
        {
            if (ModelState.IsValid)
            {
                _context.Add(sanPham);
                await _context.SaveChangesAsync();
                sanPham.HinhAnh = UploadAvatar(sanPham.ID, file);
                _context.Update(sanPham);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DanhMucID"] = new SelectList(_context.DanhMucs, "ID", "TenDanhMuc", sanPham.DanhMucID);
			var DanhMuc = _context.DanhMucs.ToList();
			ViewData["DanhMuc"] = DanhMuc;
			if (IsLogin)
			{
				var user = await _context.Users.FirstOrDefaultAsync(m => m.TaiKhoan == CurrentUser);
				ViewData["User"] = user;
			}
			return View(sanPham);
        }

        // GET: QLSanPham/Edit/5
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
            ViewData["DanhMucID"] = new SelectList(_context.DanhMucs, "ID", "TenDanhMuc", sanPham.DanhMucID);
			var DanhMuc = _context.DanhMucs.ToList();
			ViewData["DanhMuc"] = DanhMuc;
			if (IsLogin)
			{
				var user = await _context.Users.FirstOrDefaultAsync(m => m.TaiKhoan == CurrentUser);
				ViewData["User"] = user;
			}
			return View(sanPham);
        }

        // POST: QLSanPham/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,TenSanPham,Gia,MoTa,DanhMucID,HinhAnh")] SanPham sanPham, IFormFile? file)
        {
            if (id != sanPham.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (file == null)
                    {
                        _context.Update(sanPham);
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        DeleteAvatar(sanPham.ID);
                        sanPham.HinhAnh = UploadAvatar(sanPham.ID, file);
                        _context.Update(sanPham);
                        await _context.SaveChangesAsync();
                    }

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
            ViewData["DanhMucID"] = new SelectList(_context.DanhMucs, "ID", "TenDanhMuc", sanPham.DanhMucID);
            return View(sanPham);
        }

        // GET: QLSanPham/Delete/5
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

        // POST: QLSanPham/Delete/5
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
