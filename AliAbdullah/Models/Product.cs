using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AliAbdullah.Models
{
	public class Product
	{
		public int Id { get; set; }

		[Required, StringLength(200)]
		public string Name { get; set; } = "";

		[Range(0, 999999)]
		[Column(TypeName = "decimal(18,2)")]
		public decimal Price { get; set; }

		[DataType(DataType.Date)]
		public DateTime CreationDate { get; set; }

		[Required]
		public int ServiceProviderId { get; set; }
		public ServiceProvider? ServiceProvider { get; set; }
	}
}
