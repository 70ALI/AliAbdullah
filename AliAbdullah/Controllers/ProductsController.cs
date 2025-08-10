using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AliAbdullah.Data;
using AliAbdullah.Models;

namespace AliAbdullah.Controllers
{
	public class ProductsController : Controller
	{
		private readonly AppDbContext _context;
		public ProductsController(AppDbContext context) => _context = context;

		[HttpGet]
		public async Task<IActionResult> Index(ProductFilterVm? f, string? reset)
		{
			var forceOpen = !string.IsNullOrEmpty(reset);
			f ??= new ProductFilterVm();
			if (forceOpen) f = new ProductFilterVm();

			var q = _context.Products
				.Include(p => p.ServiceProvider)
				.AsQueryable();

			if (!string.IsNullOrWhiteSpace(f.Name))
				q = q.Where(p => EF.Functions.Like(p.Name, $"%{f.Name}%"));

			if (f.MinPrice is not null) q = q.Where(p => p.Price >= f.MinPrice);
			if (f.MaxPrice is not null) q = q.Where(p => p.Price <= f.MaxPrice);

			var from = f.From?.Date;
			var toExcl = f.To?.Date.AddDays(1);
			if (from is not null) q = q.Where(p => p.CreationDate >= from);
			if (toExcl is not null) q = q.Where(p => p.CreationDate < toExcl);

			if (f.ServiceProviderId is not null) q = q.Where(p => p.ServiceProviderId == f.ServiceProviderId);

			f.Results = await q.OrderByDescending(p => p.CreationDate).ToListAsync();

			ViewData["ServiceProviderId"] =
				new SelectList(_context.ServiceProviders.OrderBy(x => x.Name), "Id", "Name", f.ServiceProviderId);

			ViewBag.ForceOpen = forceOpen;
			return View(f);
		}


		[HttpGet]
		public IActionResult ProductFilter(ProductFilterVm f) =>
			RedirectToAction(nameof(Index), new { f.Name, f.MinPrice, f.MaxPrice, f.From, f.To, f.ServiceProviderId });

		public async Task<IActionResult> Details(int? id)
		{
			if (id == null) return NotFound();

			var product = await _context.Products
				.Include(p => p.ServiceProvider)
				.FirstOrDefaultAsync(m => m.Id == id);

			if (product == null) return NotFound();

			return View(product);
		}

		public IActionResult Create()
		{
			ViewData["ServiceProviderId"] = new SelectList(_context.ServiceProviders.OrderBy(x => x.Name), "Id", "Name");
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create([Bind("Id,Name,Price,CreationDate,ServiceProviderId")] Product product)
		{
			if (ModelState.IsValid)
			{
				_context.Add(product);
				await _context.SaveChangesAsync();
				return RedirectToAction(nameof(Index));
			}
			ViewData["ServiceProviderId"] = new SelectList(_context.ServiceProviders.OrderBy(x => x.Name), "Id", "Name", product.ServiceProviderId);
			return View(product);
		}

		public async Task<IActionResult> Edit(int? id)
		{
			if (id == null) return NotFound();

			var product = await _context.Products.FindAsync(id);
			if (product == null) return NotFound();

			ViewData["ServiceProviderId"] = new SelectList(_context.ServiceProviders.OrderBy(x => x.Name), "Id", "Name", product.ServiceProviderId);
			return View(product);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Price,CreationDate,ServiceProviderId")] Product product)
		{
			if (id != product.Id) return NotFound();

			if (ModelState.IsValid)
			{
				try
				{
					_context.Update(product);
					await _context.SaveChangesAsync();
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!ProductExists(product.Id)) return NotFound();
					throw;
				}
				return RedirectToAction(nameof(Index));
			}
			ViewData["ServiceProviderId"] = new SelectList(_context.ServiceProviders.OrderBy(x => x.Name), "Id", "Name", product.ServiceProviderId);
			return View(product);
		}

		public async Task<IActionResult> Delete(int? id)
		{
			if (id == null) return NotFound();

			var product = await _context.Products
				.Include(p => p.ServiceProvider)
				.FirstOrDefaultAsync(m => m.Id == id);

			if (product == null) return NotFound();

			return View(product);
		}

		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			var product = await _context.Products.FindAsync(id);
			if (product != null) _context.Products.Remove(product);
			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}

		private bool ProductExists(int id) => _context.Products.Any(e => e.Id == id);
	}
}
