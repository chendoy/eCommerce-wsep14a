using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce_14a
{
    public class Cart
    {
        List<Basket> baskets;

        public Cart()
        {
            baskets = new List<Basket>();
        }
        public List<Basket> ViewDetails()
        {
            return baskets;
        }

        public bool AddBasket(Basket basket) 
        {
            baskets.Add(basket);
            return true;
        }

        public bool RemoveAllBaskets() 
        {
            baskets.Clear();
            return true;
        }
    }
}
