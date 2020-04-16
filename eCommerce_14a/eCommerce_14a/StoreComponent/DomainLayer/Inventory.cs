using System;
using System.Collections.Generic;
using eCommerce_14a.Utils;

namespace eCommerce_14a.StoreComponent.DomainLayer
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

        public Tuple<bool, string> appendProduct(Dictionary<string, object> productParams, int amount)
        {
            if (amount < 0)
                return new Tuple<bool, string>(false, CommonStr.InventoryErrorMessage.NegativeProductAmountErrMsg);

            int pId = (int)productParams[CommonStr.ProductParams.ProductId];
            if (invProducts.ContainsKey(pId))
                return new Tuple<bool, string>(false, CommonStr.InventoryErrorMessage.ProductAlreadyExistErrMsg);

            double pPrice = (double)productParams[CommonStr.ProductParams.ProductPrice];
            if (pPrice < 0)
                return new Tuple<bool, string>(false, CommonStr.InventoryErrorMessage.ProductPriceErrMsg);

            string pDetails = (string)productParams[CommonStr.ProductParams.ProductDetails];
            string pName = (string)productParams[CommonStr.ProductParams.ProductName];
            string pCategory = (string)productParams[CommonStr.ProductParams.ProductCategory];

            Product product = new Product(product_id: pId, details: pDetails, name: pName, category: pCategory);
            invProducts.Add(pId, new Tuple<Product, int>(product, amount));

            return new Tuple<bool, string>(true, "");
        }

        public Tuple<bool, string> removeProduct(int productId)
        {
            if (!invProducts.ContainsKey(productId))
                return new Tuple<bool, string>(false, CommonStr.InventoryErrorMessage.ProductNotExistErrMsg);
            if (invProducts.Remove(productId))
                return new Tuple<bool, string>(true, "");
            else
                return new Tuple<bool, string>(false, CommonStr.GeneralErrMessage.UnKnownErr);

        }

        public Tuple<bool, string> UpdateProduct(Dictionary<string, object> productParams)
        {
            int productId = (int)productParams[CommonStr.ProductParams.ProductId];
            if (!invProducts.ContainsKey(productId))
                return new Tuple<bool, string>(false, CommonStr.InventoryErrorMessage.ProductNotExistErrMsg);

            double price = (double)productParams[CommonStr.ProductParams.ProductPrice];
            if (price < 0)
                return new Tuple<bool, string>(false, CommonStr.InventoryErrorMessage.ProductPriceErrMsg);

            Product product = invProducts[productId].Item1;
            product.Details = (string)productParams[CommonStr.ProductParams.ProductDetails];
            product.Price = price;
            product.Name = (string)productParams[CommonStr.ProductParams.ProductName];
            product.Category = (string)productParams[CommonStr.ProductParams.ProductCategory];
            int amount = invProducts[productId].Item2;

            invProducts[productId] = new Tuple<Product, int>(product, amount);

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


        //return the product and it's amount in the inventory
        public Tuple<Product, int> getProductDetails(int productId)
        {
            if (!invProducts.ContainsKey(productId))
                return null;
            else
                return invProducts[productId];
        }


        public static Tuple<bool, string> isValidInventory(Dictionary<int, Tuple<Product, int>> inv)
        {
            if (inv == null)
            {
                return new Tuple<bool, string>(false, CommonStr.InventoryErrorMessage.NullInventroyErrMsg);
            }
            //checking the amount of each product is not negative and each key matches the product id
            foreach (KeyValuePair<int, Tuple<Product, int>> entry in inv)
            {
                int productId = entry.Value.Item1.ProductID;
                int productAmount = entry.Value.Item2;
                if (productAmount < 0)
                {
                    return new Tuple<bool, string>(false, CommonStr.InventoryErrorMessage.NegativeProductAmountErrMsg);

                }
                if (productId != entry.Key)
                {
                    return new Tuple<bool, string>(false, CommonStr.InventoryErrorMessage.UnmatchedProductAnKeyErrMsg);

                }
            }

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
        
     
        public Tuple<bool, string> isValidBasket(Dictionary<int, int> basket)
        {
           foreach(KeyValuePair<int, int> entry in basket)
            {
                int id = entry.Key;
                int amount = entry.Value;
                if (!invProducts.ContainsKey(id))
                    return new Tuple<bool, string>(false, CommonStr.InventoryErrorMessage.ProductNotExistErrMsg + " - PID : " + id.ToString());

                int invProductAmount = invProducts[id].Item2;
                if (invProductAmount < amount)
                    return new Tuple<bool, string>(false, CommonStr.InventoryErrorMessage.ProductShortageErrMsg + " - PID : " +  id.ToString());
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
                    basketPrice += this.invProducts[prod_id].Item1.Price * amount;
            }

            return basketPrice;
        }

        public bool productExist(int productId)
        {
            return invProducts.ContainsKey(productId);
        }
    }
}