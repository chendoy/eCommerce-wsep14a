using System;
using System.Collections.Generic;

namespace eCommerce_14a
{
    public class Inventory
    {
        private Dictionary<int, Tuple<Product, int>> invProducts;

        public Inventory()
        {
            this.invProducts = new Dictionary<int, Tuple<Product, int>>();
        }

        public Dictionary<int, Tuple<Product, int>> Inv
        {
            get { return invProducts; }
        }


        
      
     
        public Tuple<bool, string> removeProduct(int productId)
        {
            if (!invProducts.ContainsKey(productId))
                return new Tuple<bool, string>(false, CommonStr.InventoryErrorMessage.ProductNotExistErrMsg);

            invProducts.Remove(productId);
            return new Tuple<bool, string>(true, "");
        }

        public Tuple<bool, string> addProductAmount(int productId, int amount)
        {
            // purpose: add amount to the existing amount of product
            // return: on sucess <true,null> , on failing <false, excpection>
            if (amount < 0)
                return new Tuple<bool, string>(false, CommonStr.InventoryErrorMessage.productAmountErrMsg);

            if (!invProducts.ContainsKey(productId))
                return new Tuple<bool, string>(false, CommonStr.InventoryErrorMessage.ProductNotExistErrMsg);

            int currentAmount = invProducts[productId].Item2;
            invProducts[productId] = new Tuple<Product, int>(invProducts[productId].Item1, currentAmount + amount);

            return new Tuple<bool, string>(true, "");
        }



        public Tuple<bool, string> DecraseProductAmount(int productId, int amount)
        {
            // purpose: decrase amount from the existing amount of product
            // return: on sucess <true,null> , on failing <false, excpection>
            if (amount < 0)
                return new Tuple<bool, string>(false, CommonStr.InventoryErrorMessage.productAmountErrMsg);

            if(!invProducts.ContainsKey(productId))
                return new Tuple<bool, string>(false, CommonStr.InventoryErrorMessage.ProductNotExistErrMsg);

            int currentAmount = invProducts[productId].Item2;
            int newAmount = currentAmount - amount;
            if (newAmount < 0)
                return new Tuple<bool, string>(false, CommonStr.InventoryErrorMessage.productAmountErrMsg);

            invProducts[productId] = new Tuple<Product, int>(invProducts[productId].Item1, newAmount);

            return new Tuple<bool, string>(true, "");
        }

        public Tuple<bool, string> UpdateProduct(Dictionary<string, object> productParams)
        {
            int productId = (int)productParams["Id"];
            if (!invProducts.ContainsKey(productId))
                return new Tuple<bool, string>(false, CommonStr.InventoryErrorMessage.ProductNotExistErrMsg);

            double price = (double)productParams["Price"];
            if (price < 0)
                return new Tuple<bool, string>(false, CommonStr.InventoryErrorMessage.ProductPriceErrMsg);

            Product product = invProducts[productId].Item1;
            product.Details = (string)productParams["pDetails"];
            product.Price = price;
            product.Name = (string)productParams["pName"];
            product.Category = (string)productParams["pCategory"];
            int amount = invProducts[productId].Item2;

            invProducts[productId] = new Tuple<Product, int>(product, amount);

            return new Tuple<bool, string>(true, "");
        }

        public Tuple<bool,string> appendProduct(Dictionary<string, object> productParams, int amount)
        {
            if (amount < 0)
                return new Tuple<bool, string>(false, CommonStr.InventoryErrorMessage.productAmountErrMsg);

            int pId = (int)productParams["Id"];
            if (invProducts.ContainsKey(pId))
                return new Tuple<bool, string>(false, CommonStr.InventoryErrorMessage.ProductAlreadyExistErrMsg);
            
            double pPrice = (double)productParams["Price"];
            if (pPrice < 0)
                return new Tuple<bool, string>(false, CommonStr.InventoryErrorMessage.ProductPriceErrMsg);

            string pDetails = (string)productParams["pDetails"];
            string pName = (string)productParams["pName"];
            string pCategory = (string)productParams["pCategory"];

            Product product = new Product (product_id:pId, details: pDetails, name:pName, category:pCategory);
            invProducts.Add(pId, new Tuple<Product, int>(product, amount));

            return new Tuple<bool, string>(true, "");
        }

        public Tuple<bool, string> loadInventory(Dictionary<int, Tuple<Product, int>> otherInv)
        {
            Tuple<bool, string> isValidAns = isValidInventory(otherInv);
            bool isValid = isValidAns.Item1;
            if (isValid)
            {
                invProducts = otherInv;
                return new Tuple<bool, string>(true, null);
            }
            else
            {
                return new Tuple<bool, string>(false, isValidAns.Item2);
            }
        }
        
        //return the product and it's amount in the inventory
        public Tuple<Product,int> getProductDetails(int productId)
        {
            if (!invProducts.ContainsKey(productId))
                return null;
            else
                return invProducts[productId];
        }

        public static Tuple <bool,string> isValidInventory(Dictionary<int, Tuple<Product, int>> inv)
        {
            if (inv == null)
            {
                return new Tuple<bool, string>(false, "cann't set the current inventory to null");
            }
            //checking the amount of each product is not negative and each key matches the product id
            foreach (KeyValuePair<int, Tuple<Product, int>> entry in inv)
            { 
                int productId = entry.Value.Item1.ProductID;
                int productAmount = entry.Value.Item2;
                if (productAmount < 0)
                {
                    return new Tuple<bool, string>(false, "product amount in inventroy can't be negative");

                }
                if(productId != entry.Key)
                {
                    return new Tuple<bool, string>(false, "product id must match it's key");

                }
            }

            return new Tuple<bool, string>(true, "");
        }

        public Tuple<bool, string> isValidBasket(Dictionary<int, int> products)
        {
           foreach(KeyValuePair<int, int> entry in products)
            {
                int id = entry.Key;
                int amount = entry.Value;
                if (!this.invProducts.ContainsKey(id))
                    return new Tuple<bool, string>(false, CommonStr.InventoryErrorMessage.ProductNotExistErrMsg + " " + id.ToString());

                int invProductAmount = invProducts[id].Item2;
                if (invProductAmount < amount)
                    return new Tuple<bool, string>(false, CommonStr.InventoryErrorMessage.ProductShortageErrMsg + " " + id.ToString());
            }

            return new Tuple<bool, string>(true, "");
        }

        public double getBasketPrice(Dictionary<int, int> products)
        {
            double basketPrice = 0;
            foreach(KeyValuePair<int, int> entry in products)
            {
                int prod_id = entry.Key;
                int amount = entry.Value;
                if (!this.invProducts.ContainsKey(prod_id))
                    return -1;
                else
                    basketPrice += this.invProducts[prod_id].Item1.Price;
            }

            return basketPrice;
        }

        public bool productExist(int productId)
        {
            return invProducts.ContainsKey(productId);
        }
    }
}