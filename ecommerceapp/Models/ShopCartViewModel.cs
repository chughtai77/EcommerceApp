namespace ecommerceapp.Models
{
	public class ShopCartViewModel
	{
		public ShopCartViewModel()
		{
			ListCart = new List<ShopCartVM>();
		}

		public List<ShopCartVM> ListCart { get; set; }

		public List<ShoppingCart> CartItems { get; set; }

        


        public decimal? TotalPrice
		{
			get { return ListCart.Sum(c => c.count * c.Product.productPrice); }
		}

        public int Quantity { get; set; }

		public decimal? TotalPrice2
		{
			get { return CartItems.Sum(c => c.Quantity * c.Product.productPrice); }
		}

		//public decimal? TotalPrice2
		//{
		//    get { return CartItems.Where(c => c.Product != null).Sum(c => c.Quantity * c.Product.productPrice); }
		//}

		public ShoppingCart SCart { get; set; }
        public ShopCartVM SCartVM { get; set; }

        

        public void IncrementCount(int itemId)
		{
			var item = ListCart.FirstOrDefault(i => i.Id == itemId);
			if (item != null)
			{
				item.count++;
			}
		}


		//Decrement the count of the item with the specified ID
		public void DecrementCount(int itemId)
		{
			var item = ListCart.FirstOrDefault(i => i.Id == itemId);
			if (item != null && item.count > 1)
			{
				item.count--;
			}
			else if (item != null && item.count == 1)
			{
				ListCart.Remove(item);
			}

		}

	}
}
