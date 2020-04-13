using System;
using System.Collections.Generic;

namespace eCommerce_14a
{
    public class Inventory
    {
        private Dictionary<int, Tuple<Product, int>> inv;
        string productPriceErrMessage = "product price invalid";
        string productExistErrMessage = "this product already exists!, you can update it's amount!";
        string productNotExistErrMessage = "prdouct not exist!";
        string productAmountErrMessage = "amount must be greater than 0";
        string nullProductErrMessage = "product cann't be null";
        public Inventory()
        {
            this.inv = new Dictionary<int, Tuple<Product, int>>();
        }

        public Dictionary<int, Tuple<Product, int>> Inv
        {
            get { return inv; }
        }


        
        public Tuple<bool, string> addProductAmount(int productId, int amount)
        {
            // purpose: add amount to the existing amount of product
            // return: on sucess <true,null> , on failing <false, excpection>
            if (amount < 0)
                return new Tuple<bool, string>(false, productAmountErrMessage);
             
            if (!inv.ContainsKey(productId))
                return new Tuple<bool, string>(false, productNotExistErrMessage);

            int currentAmount = inv[productId].Item2;
            inv[productId] = new Tuple<Product, int>(inv[productId].Item1, currentAmount + amount);

            return new Tuple<bool, string>(true, "");
        }

        

        public Tuple<bool, string> UpdateProductDetails(int productId, string newDetails)
        {

            if (newDetails == null)
                return new Tuple<bool, string>(false, "details cann't b null");
            if (!inv.ContainsKey(productId))
            {
                return new Tuple<bool, string>(false, "this product not exists in the Inventory");
            }
            inv[productId].Item1.Details = newDetails;
            return new Tuple<bool, string>(true, "");
        }

        public Tuple<bool, string> removeProduct(int productId)
        {
            if (!inv.ContainsKey(productId))
                return new Tuple<bool, string>(false, productNotExistErrMessage);

            inv.Remove(productId);
            return new Tuple<bool, string>(true, "");
        }

        public Tuple<bool, string> DecraseProductAmount(int productId, int amount)
        {
            // purpose: decrase amount from the existing amount of product
            // return: on sucess <true,null> , on failing <false, excpection>
            if (amount < 0)
                return new Tuple<bool, string>(false, productAmountErrMessage);

            if(!inv.ContainsKey(productId))
                return new Tuple<bool, string>(false, productNotExistErrMessage);

            int currentAmount = inv[productId].Item2;
            int newAmount = currentAmount - amount;
            if (newAmount < 0)
                return new Tuple<bool, string>(false, productAmountErrMessage);

            inv[productId] = new Tuple<Product, int>(inv[productId].Item1, newAmount);

            return new Tuple<bool, string>(true, "");
        }

        public Tuple<bool, string> UpdateProduct(Dictionary<string, object> productParams)
        {
            int productId = (int)productParams["Id"];
            if (!inv.ContainsKey(productId))
                return new Tuple<bool, string>(false, productNotExistErrMessage);

            double price = (double)productParams["Price"];
            if (price < 0)
                return new Tuple<bool, string>(false, productPriceErrMessage);

            Product product = inv[productId].Item1;
            product.Details = (string)productParams["pDetails"];
            product.Price = price;
            product.Name = (string)productParams["pName"];
            product.Category = (string)productParams["pCategory"];
            int amount = inv[productId].Item2;

            inv[productId] = new Tuple<Product, int>(product, amount);

            return new Tuple<bool, string>(true, "");
        }

        public Tuple<bool,string> appendProduct(Dictionary<string, object> productParams, int amount)
        {
            if (amount < 0)
                return new Tuple<bool, string>(false, productAmountErrMessage);

            int pId = (int)productParams["Id"];
            if (inv.ContainsKey(pId))
                return new Tuple<bool, string>(false, productExistErrMessage);
            
            double pPrice = (double)productParams["Price"];
            if (pPrice < 0)
                return new Tuple<bool, string>(false, productPriceErrMessage);

            string pDetails = (string)productParams["pDetails"];
            string pName = (string)productParams["pName"];
            string pCategory = (string)productParams["pCategory"];

            Product product = new Product (product_id:pId, details: pDetails, name:pName, category:pCategory);
            inv.Add(pId, new Tuple<Product, int>(product, amount));

            return new Tuple<bool, string>(true, "");
        }

        public Tuple<bool, string> loadInventory(Dictionary<int, Tuple<Product, int>> otherInv)
        {
            Tuple<bool, string> isValidAns = isValidInventory(otherInv);
            bool isValid = isValidAns.Item1;
            if (isValid)
            {
                inv = otherInv;
                return new Tuple<bool, string>(true, null);
            }
            else
            {
                return new Tuple<bool, string>(false, isValidAns.Item2);
            }
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
    }
}