using ecommerceapp.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ecommerceapp.Data
{
	public class ApplicationDbContext :  IdentityDbContext
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
		{

		}

		public DbSet<Categories> category { get; set; }
        public DbSet<Product> product { get; set; }

		public DbSet<ShoppingCart> shoppingCarts { get; set; }

        public DbSet<ShopCartVM> shopCartVMs { get; set; }

        public DbSet<Register> Registers { get; set; }

	
	}
}
