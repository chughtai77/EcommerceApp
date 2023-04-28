using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace ecommerceapp.Models
{
	public class Categories
	{
		[Key]
		public int CategoryId { get; set; }
		[Required]
		public string CategoryName { get; set; }
		[AllowNull]
		public string img { get; set; }

		[NotMapped]
		public IFormFile imgfile { get; set; }

		[NotMapped]
        public ICollection<Product> products { get; set; }

        

    }
}
