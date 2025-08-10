using System.ComponentModel.DataAnnotations;

namespace AliAbdullah.Models
{
	public class ProductFilterVm : IValidatableObject
	{
		public string? Name { get; set; }
		public decimal? MinPrice { get; set; }
		public decimal? MaxPrice { get; set; }
		public DateTime? From { get; set; }
		public DateTime? To { get; set; }
		public int? ServiceProviderId { get; set; }

		public IEnumerable<Product>? Results { get; set; }


		public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
		{
			if (MinPrice is not null && MaxPrice is not null && MinPrice > MaxPrice)
				yield return new ValidationResult("Min Price must be ≤ Max Price", new[] { nameof(MinPrice), nameof(MaxPrice) });

			if (From is not null && To is not null && From > To)
				yield return new ValidationResult("From must be ≤ To", new[] { nameof(From), nameof(To) });
		}
	}
}
