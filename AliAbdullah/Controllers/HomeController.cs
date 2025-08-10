using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AliAbdullah.Data;
using AliAbdullah.Models;

namespace AliAbdullah.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
		private readonly AppDbContext _db;

		public HomeController(ILogger<HomeController> logger, AppDbContext db)
		{
			_logger = logger;
			_db = db;
		}

		public async Task<IActionResult> Index()
		{
			ViewBag.Providers = await _db.ServiceProviders.CountAsync();
			ViewBag.Products = await _db.Products.CountAsync();
			ViewBag.LastDate = await _db.Products.MaxAsync(p => (DateTime?)p.CreationDate);
			return View();
		}

		public IActionResult Privacy() => View();

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error() =>
			View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
	}
}
