using ShopingCartEF;
using System.Collections.Generic;
using System.Linq;

namespace ShoppingCartMvc.Models
{
    public class CartItem
    {
        public Product Product { get; set; }
        public int Quantity { get; set; }
       
    public double Total
        {
            get
            {
                return Product.Price * Quantity;
            }
        }
    }
    public class MyCart
    {
        private List<CartItem> listCartItems = new List<CartItem>();
        public List<CartItem> MyCarts
        {
            get { return listCartItems; }
        }
        public bool IsCartEmpty
        {
            get
            {
                return listCartItems.Count == 0;
            }
        }
        // Vừa them, vua cap nhat so luong 
     
        public void AddToCart(Product p, int quantity)
        {
            var product = MyCarts.Where(x => x.Product.ProductsID == p.ProductsID).FirstOrDefault();
            if (product == null)
            {
                MyCarts.Add(new CartItem { Product = p, Quantity= quantity});
            }
            else
            {
                product.Quantity += quantity;
            }
        }
        public void RemoveFromCart(Product p)
        {
            MyCarts.RemoveAll(x => x.Product.ProductsID.Equals(p.ProductsID));
        }
        public double SubTotal()
        {
            return MyCarts.Sum(x => x.Total);
        }
    }
}