using BaitapLon.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Web;

namespace BaitapLon.Controllers
{
	public class CartController : BaseController
	{
		private readonly BTLContext _context;

		public CartController(BTLContext context)
		{
			_context = context;
		}
		// GET: Cart
		public async Task<IActionResult> Index()
		{
			List<Cart> listCart = ListCart;
			var DanhMuc = _context.DanhMucs.ToList();
			ViewData["DanhMuc"] = DanhMuc;
			if (IsLogin)
			{
				var user = await _context.Users.FirstOrDefaultAsync(m => m.TaiKhoan == CurrentUser);
				ViewData["User"] = user;
			}
			else
			{
				return RedirectToAction("Login", "Users");
;			}
			return View(listCart);
		}

		public async Task<IActionResult> ThemSP(int id)
		{
			if (!IsLogin)
			{
				return RedirectToAction("Login", "Users");
			}
			var sanpham = _context.SanPhams.FirstOrDefault(m => m.ID == id);
			List<Cart> listCart = ListCart;
			if(listCart == null)
			{
				List<Cart> listcarts = new List<Cart>();
				Cart cart = new Cart();
				cart.SanPham = sanpham;
				cart.SoLuong = 1;
				listcarts.Add(cart);
				ListCart = listcarts;
				return RedirectToAction("Index");
			}
			else
			{
				bool check = false;
				foreach(var cart in listCart)
				{
					if(cart.SanPham.ID == id)
					{
						cart.SoLuong = cart.SoLuong + 1;
						check = true;
					}
				}
				if(check == false)
				{
					Cart cart = new Cart();
					cart.SanPham = sanpham;
					cart.SoLuong = 1;
					listCart.Add(cart);
				}
				ListCart = listCart;
				return RedirectToAction("Index");
			}
		}
		public async Task<IActionResult> Tru(int id)
		{
			if (ListCart == null)
			{
				return Problem("Entity set 'BTLContext.SanPhams'  is null.");
			}
			List<Cart> listcart = ListCart;
			if (listcart != null)
			{
				foreach (var cart in listcart)
				{
					if (cart.SanPham.ID == id)
					{
						cart.SoLuong = cart.SoLuong - 1; 
						if(cart.SoLuong==0)
						{
							return RedirectToAction("Delete", new { id = cart.SanPham.ID });
						}
						ListCart = listcart;
						return RedirectToAction("Index");
					}
					
				}
			}
			return RedirectToAction("Index");
		}
		public async Task<IActionResult> Cong(int id)
		{
			if (ListCart == null)
			{
				return Problem("Entity set 'BTLContext.SanPhams'  is null.");
			}
			List<Cart> listcart = ListCart;
			if (listcart != null)
			{
				foreach (var cart in listcart)
				{
					if (cart.SanPham.ID == id)
					{
						cart.SoLuong = cart.SoLuong + 1;
						ListCart = listcart;
						return RedirectToAction("Index");
					}
				}
			}
			return RedirectToAction("Index");
		}
		public async Task<IActionResult> Delete(int id)
		{
			if (ListCart == null)
            {
                return Problem("Entity set 'BTLContext.SanPhams'  is null.");
            }
			List<Cart> listcart = ListCart;
            if (listcart != null)
            {
				foreach(var cart in listcart)
				{
					if(cart.SanPham.ID == id)
					{
						listcart.Remove(cart);
						ListCart = listcart;
						return RedirectToAction("Index");
					}
				}
			}
            return RedirectToAction("Index");
		}

	}
}
