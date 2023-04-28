using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace ecommerceapp.Models
{
    public class Product
    {
        [Key]
        public int productId { get; set; }
        [Required]
        public string productName { get; set; }
        [Required]
        [StringLength(75)]
        public string productDesc { get; set; }
        
        public int productPrice { get; set; }

        [AllowNull]
        public string prodimg { get; set; }

        [NotMapped]
        public IFormFile imgfile { get; set; }

        [NotMapped]
		public IEnumerable<Product> prodlist { get; set; }


        //------------------------------
        
        public int cId { get; set; }
        [AllowNull]
        [NotMapped]
        public string category { get; set; }

        //-----------------------------
        [AllowNull]
        [NotMapped]
        public IEnumerable<Categories> categories { get; set; }
    }
}
