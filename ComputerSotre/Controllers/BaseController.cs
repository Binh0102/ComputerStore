using BaitapLon.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;

namespace BaitapLon.Controllers
{
	public class BaseController : Controller
	{
		public string CurrentUser
		{
			get
			{
				return HttpContext.Session.GetString("USER_NAME");
			}
			set
			{
				HttpContext.Session.SetString("USER_NAME", value);
			}
		}
		public bool IsLogin
		{
			get
			{
				return !string.IsNullOrEmpty(CurrentUser);
			}
		}
		
		public List<Cart> ListCart
		{
			get
			{
				return HttpContext.Session.GetObject("listcart");
			}
			set
			{
				HttpContext.Session.SetObject("listcart", value);
			}
		}

	}
}
