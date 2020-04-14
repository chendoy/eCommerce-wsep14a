using System;
using System.Collections.Generic;

namespace eCommerce_14a
{
    public class Searcher
    {
       

        private StoreManagment storeManagemnt;
        public Searcher(StoreManagment storeManagment)
        {
            this.storeManagemnt = storeManagment;
        }
        /// <test cref ="TestingSystem.UnitTests(Dictionary{string, object})"/>
        // the functions search products on all stores and returns List of <storeId, List<matches products>> that matches the filters
        public Dictionary<int, List<Product>> SearchProducts(Dictionary<string, object> searchBy)
        {
            Dictionary<int, Store> activeStores = storeManagemnt.getActiveSotres();
            Dictionary<int,List<Product>> matchProducts = new Dictionary<int, List<Product>>();
            foreach (KeyValuePair<int, Store> entry in activeStores)
            {
                Store store = entry.Value;
                Inventory storeInv = store.Inventory;

                if(ValidStoreRank(store, searchBy))
                {
                    foreach (KeyValuePair<int, Tuple<Product, int>> entryProduct in storeInv.Inv)
                    {
                        Product product = entryProduct.Value.Item1;

                        if (ValidProductName(product, searchBy))
                        {
                            if (ValidProductCategory(product, searchBy))
                            {

                                if (ValidPriceRange(product, searchBy))
                                {

                                    if (ValidProductRank(product, searchBy))
                                    {

                                        if (ValidProductKeyWord(product, searchBy))
                                        {
                                            if (matchProducts.ContainsKey(store.Id))
                                                matchProducts[store.Id].Add(product);
                                            else
                                            matchProducts.Add(store.Id, new List<Product> { product });

                                        }

                                    }

                                }
                            } 
                           
                        }

                    }

                }
            }
                
            return matchProducts;
        }

        private bool ValidProductKeyWord(Product product, Dictionary<string, object> searchBy)
        {

            if (searchBy.ContainsKey(CommonStr.ProductKeyWord))
            {
                string filterkeyWord = searchBy[CommonStr.ProductKeyWord].ToString().Replace(" ","").ToLower();
                string productName = product.Name.Replace(" ", "").ToLower();
                if (!productName.Contains(filterkeyWord))
                    return false;
            }

            return true;
        }

        private bool ValidProductRank(Product product, Dictionary<string, object> searchBy)
        {
            if (searchBy.ContainsKey(CommonStr.ProductRank))
            {
                int minRank = (int)searchBy[CommonStr.ProductRank];

                if (product.Rank < minRank)
                    return false;
            }
            return true;
        }

        private bool ValidPriceRange(Product product, Dictionary<string, object> searchBy)
        {
            if (searchBy.ContainsKey(CommonStr.ProductPriceRange))
            {
                Tuple<double, double> priceRange = (Tuple<double, double>)searchBy[CommonStr.ProductPriceRange];
                double minPrice = priceRange.Item1;
                double maxPrice = priceRange.Item2;


                if (product.Price > maxPrice || product.Price < minPrice)
                    return false;
            }
            return true;
        }

        private bool ValidProductCategory(Product product, Dictionary<string, object> searchBy)
        {

            if (searchBy.ContainsKey(CommonStr.ProductCategory))
            {
                string filterCategory = searchBy[CommonStr.ProductCategory].ToString().Replace(" ", "").ToLower();
                string productCategory = product.Category.Replace(" ", "").ToLower();

                if (!filterCategory.Contains(productCategory) && !productCategory.Contains(filterCategory))
                    return false;
            }

            return true;
        }

        private bool ValidStoreRank(Store store, Dictionary<string, object> searchBy)
        {

            if (searchBy.ContainsKey(CommonStr.StoreRank))
            {
                int minRank = (int)searchBy[CommonStr.StoreRank];
                if (store.Rank < minRank)
                    return false;
            }
            return true;
        }

        private bool ValidProductName(Product product, Dictionary<string, object> searchBy)
        {
            if (searchBy.ContainsKey(CommonStr.ProductName))
            {
                string filtrProuctName = (string)searchBy[CommonStr.ProductName];
                filtrProuctName = filtrProuctName.Replace(" ", "").ToLower();
                string curProdName = product.Name.Replace(" ", "").ToLower();
                if (!filtrProuctName.Equals(curProdName))
                    return false;
            }
            return true;
        }

   



    }
}

