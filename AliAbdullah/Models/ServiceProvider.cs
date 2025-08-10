using System.ComponentModel.DataAnnotations;

namespace AliAbdullah.Models
{
	public class ServiceProvider
	{
		public int Id { get; set; }

		[Required, StringLength(150)]
		public string Name { get; set; } = "";

		[EmailAddress, StringLength(150)]
		public string? ContactEmail { get; set; }

		[Phone, StringLength(50)]
		public string? Phone { get; set; }

		public ICollection<Product> Products { get; set; } = new List<Product>();
	}
}
