using Microsoft.Build.Framework;

namespace ecommerceapp.Models
{
	public class OrderHeader
	{
		public int Id { get; set; }

		[Required]
		public DateTime OrderDate { get; set; }
		public DateTime ShippingDate { get; set; }

		public double Ordertotal { get; set; }
		public string? OrderStatus { get; set; }
		public string? PaymentStatus { get; set; }
		public string? TrackerNumber { get; set; }
		public string? Carrier { get; set; }
		public DateTime PaymentDate { get; set; }
		public DateTime PaymentDueDate { get; set; }

		public string? SessionId { get; set; }
		public string? PaymentIntentId { get; set; }

		[Required]
		public string Name { get; set; }
		[Required]
		public string Email { get; set; }
		[Required]
		public string PhoneNumber { get; set; }
		[Required]
		public string Address { get; set; }



	}
}
