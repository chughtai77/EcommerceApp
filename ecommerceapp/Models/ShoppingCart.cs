using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;

namespace ecommerceapp.Models
{
    public class ShoppingCart
    {
		public int Id { get; set; }
		public int productId { get; set; }
		[ForeignKey("productId")]
		[ValidateNever]
		public Product Product { get; set; }
		public int Quantity { get; set; }

		public string RegisterUserId { get; set; }
		[ForeignKey("RegisterUserId")]
		[ValidateNever]
		public Register RegisterUser { get; set; }

		public DateTime Timestamp { get; set; } = DateTime.Now;// Set current timestamp



        [NotMapped]
		public double Price { get; set; }

		//----------------------Product------------------
		[NotMapped]
		public string productName { get; set; }

		[NotMapped]
		public string productDesc { get; set; }
		[NotMapped]
		public int productPrice { get; set; }

		[NotMapped]
		[AllowNull]
		public string prodimg { get; set; }



    }
}
