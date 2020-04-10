using System;
using System.Collections.Generic;

namespace eCommerce_14a
{
    public class Inventory
    {
        private Dictionary<int, Tuple<Product, int>> inv;
        public Inventory()
        {
            this.inv = new Dictionary<int, Tuple<Product, int>>();
        }

        public Dictionary<int, Tuple<Product, int>> Inv
        {
            get { return inv; }
        }


        
        public Tuple<bool, Exception> addProductAmount(Product p, int amount)
        {
            // purpose: add amount to the existing amount of product
            // return: on sucess <true,null> , on failing <false, excpection>
            if (amount < 0)
            {
                return new Tuple<bool, Exception>(false, new Exception("amount must be greater than 0"));
            }
            if (p == null)
            {
                return new Tuple<bool, Exception>(false, new Exception("product cann't be null"));
            }
            else
            {
                int currentAmount = 0;
                if (inv.ContainsKey(p.ProductID))
                {
                    currentAmount = inv[p.ProductID].Item2;
                    inv[p.ProductID] = new Tuple<Product, int>(p, currentAmount + amount);
                }
                else
                {
                    inv.Add(p.ProductID, new Tuple<Product, int>(p, currentAmount + amount));
                }
                return new Tuple<bool, Exception>(true, null);
            }

        }

        public Tuple<bool, Exception> UpdateProductDetails(int productId, string newDetails)
        {

            if (newDetails == null)
                return new Tuple<bool, Exception>(false, new Exception("details cann't b null"));
            if (!inv.ContainsKey(productId))
            {
                return new Tuple<bool, Exception>(false, new Exception("this product not exists in the Inventory"));
            }
            inv[productId].Item1.Details = newDetails;
            return new Tuple<bool, Exception>(true, null);
        }

        public Tuple<bool, Exception> DecraseProductAmount(Product p, int amount)
        {
            // purpose: decrase amount from the existing amount of product
            // return: on sucess <true,null> , on failing <false, excpection>
            if (amount < 0)
            {
                return new Tuple<bool, Exception>(false, new Exception("amount must be greater than 0"));
            }
            if (p == null)
            {
                return new Tuple<bool, Exception>(false, new Exception("product cann't be null"));
            }
            else
            {
                if (inv.ContainsKey(p.ProductID))
                {
                    int currentAmount = inv[p.ProductID].Item2;
                    int newAmount = currentAmount - amount;
                    if (newAmount < 0)
                    {
                        return new Tuple<bool, Exception>(false, new Exception("by decrasing this amount the new amount is less than 0 and it's illegal!"));
                    }
                    inv[p.ProductID] = new Tuple<Product, int>(p, newAmount);
                    return new Tuple<bool, Exception>(true, null);
                }
                else
                {
                    return new Tuple<bool, Exception>(false, new Exception("you cann't decrase amount of non existing product"));
                }
            }
        }

        public Tuple<bool, Exception> loadInventory(Dictionary<int, Tuple<Product, int>> otherInv)
        {
            Tuple<bool, Exception> isValidAns = isValidInventory(otherInv);
            bool isValid = isValidAns.Item1;
            if (isValid)
            {
                inv = otherInv;
                return new Tuple<bool, Exception>(true, null);
            }
            else
            {
                return new Tuple<bool, Exception>(false, isValidAns.Item2);
            }
        }


        public static Tuple <bool,Exception> isValidInventory(Dictionary<int, Tuple<Product, int>> inv)
        {
            if (inv == null)
            {
                return new Tuple<bool, Exception>(false, new Exception("cann't set the current inventory to null"));
            }
            //checking the amount of each product is not negative and each key matches the product id
            foreach (KeyValuePair<int, Tuple<Product, int>> entry in inv)
            { 
                int productId = entry.Value.Item1.ProductID;
                int productAmount = entry.Value.Item2;
                if (productAmount < 0)
                {
                    return new Tuple<bool, Exception>(false, new Exception("product amount in inventroy can't be negative"));

                }
                if(productId != entry.Key)
                {
                    return new Tuple<bool, Exception>(false, new Exception("product id must match it's key"));

                }
            }

            return new Tuple<bool, Exception>(true, null);
        }
    }
}