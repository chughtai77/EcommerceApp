namespace ecommerceapp.Models
{
	public class IndexViewModel
	{
		public List<Product> prodlist { get; set; }
		public List<Categories> categlist { get; set; }
		public List<ShoppingCart> shoppinglist { get; set; }
        public Categories Category { get; set; }
		//----------------------------------------------
		public ShopCartVM ShopCartVMSnew { get; set; }

		public List<ShopCartVM> ShopCartVMSlist { get; set; }

    }
}
