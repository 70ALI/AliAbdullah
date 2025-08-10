using AliAbdullah.Models;
using Microsoft.EntityFrameworkCore;

namespace AliAbdullah.Data
{
	public class AppDbContext : DbContext
	{
		public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

		public DbSet<Models.ServiceProvider> ServiceProviders => Set<Models.ServiceProvider>();
		public DbSet<Product> Products => Set<Product>();
	}
}
