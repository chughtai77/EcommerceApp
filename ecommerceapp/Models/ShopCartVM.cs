using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace ecommerceapp.Models
{
    public class ShopCartVM
    {
        public int Id { get; set; }

        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        
        public Product Product { get; set; }

        public int count { get; set; }

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
        
        [NotMapped]
        public double Price { get; set; }

        [NotMapped]
        public IndexViewModel indexvm { get; set; }

    }
}
