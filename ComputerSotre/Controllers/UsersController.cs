using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BaitapLon.Models;
using System.Security.Cryptography;

namespace BaitapLon.Controllers
{
    public class UsersController : BaseController
    {
        private readonly BTLContext _context;

        public UsersController(BTLContext context)
        {
            _context = context;
        }

        // GET: Users
        public async Task<IActionResult> Index()
        {
              return _context.Users != null ? 
                          View(await _context.Users.ToListAsync()) :
                          Problem("Entity set 'BTLContext.Users'  is null.");
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.ID == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

		// GET: Users/Create
		public IActionResult Login()
		{
			return View();
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Login([Bind("ID,TaiKhoan,MatKhau")] User model)
		{
			if (ModelState.IsValid)
			{
				var loginUser = await _context.Users.FirstOrDefaultAsync(m => m.TaiKhoan == model.TaiKhoan);
				if (loginUser == null)
				{
					ModelState.AddModelError("", "Đăng nhập thất bại");
					return View(model);
				}
				else
				{
					SHA256 hashMethod = SHA256.Create();
					if (Util.Cryptography.VerifyHash(hashMethod, model.MatKhau, loginUser.MatKhau))
					{
						CurrentUser = loginUser.TaiKhoan;
						return RedirectToAction("Index", "SanPhams");
					}
					else
					{
						ModelState.AddModelError("", "Đăng nhập thất bại");
						return View(model);
					}
				}
			}
			return View(model);
		}
		public IActionResult Register()
		{
			return View();
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Register([Bind("ID,TaiKhoan,MatKhau")] User model)
		{
			if (ModelState.IsValid)
			{
				SHA256 hashMethod = SHA256.Create();
				model.MatKhau = Util.Cryptography.GetHash(hashMethod, model.MatKhau);
                model.Admin = 0;
                model.KhachHang = 1;
				_context.Add(model);
				await _context.SaveChangesAsync();
				return RedirectToAction(nameof(Login));
			}
			return View(model);
		}
		public IActionResult Logout()
		{
			CurrentUser = "";
            if(ListCart != null)
            {
                ListCart = new List<Cart>();
            }
			return RedirectToAction("Index","SanPhams");
		}

		// GET: Users/Delete/5
		/*public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.ID == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Users == null)
            {
                return Problem("Entity set 'BTLContext.Users'  is null.");
            }
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(int id)
        {
          return (_context.Users?.Any(e => e.ID == id)).GetValueOrDefault();
        }*/
    }
}
