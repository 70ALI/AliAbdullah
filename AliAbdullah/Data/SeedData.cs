using AliAbdullah.Models;
using Microsoft.EntityFrameworkCore;

namespace AliAbdullah.Data
{
	public static class SeedData
	{
		public static async Task RunAsync(IServiceProvider services)
		{
			using var scope = services.CreateScope();
			var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
			await db.Database.MigrateAsync();

			if (!await db.ServiceProviders.AnyAsync())
			{
				var sp1 = new Models.ServiceProvider { Name = "Amazon", ContactEmail = "support@amazon.com", Phone = "111111111" };
				var sp2 = new Models.ServiceProvider { Name = "Microsoft", ContactEmail = "contact@microsoft.com", Phone = "222222222" };
				db.ServiceProviders.AddRange(sp1, sp2);

				db.Products.AddRange(
					new Product { Name = "Laptop", Price = 2500m, CreationDate = DateTime.UtcNow.Date.AddDays(-1), ServiceProvider = sp1 },
					new Product { Name = "Headset", Price = 199.99m, CreationDate = DateTime.UtcNow.Date.AddDays(-2), ServiceProvider = sp1 },
					new Product { Name = "Office 365", Price = 29.99m, CreationDate = DateTime.UtcNow.Date, ServiceProvider = sp2 }
				);

				await db.SaveChangesAsync();
			}
		}
	}
}
