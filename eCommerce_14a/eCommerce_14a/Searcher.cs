using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce_14a
{
    public class Searcher
    {

        private StoreManagment storeManagemnt;
        public Searcher(StoreManagment storeManagment)
        {
            this.storeManagemnt = storeManagment;
        }

        public List<Product> SearchProducts(Dictionary<string, object> searchBy)
        {
            Dictionary<int, Store> activeStores = storeManagemnt.getActiveSotres();
            List<Product> matchProducts = new List<Product>();
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
                                            matchProducts.Add(product);
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

            if (searchBy.ContainsKey("SearchByKeyWord"))
            {
                string keyWord = (string)searchBy["SearchByKeyWord"];
                string productNameNoSpaces = product.Name.Replace(" ", "");
                string lowerProdName = productNameNoSpaces.ToLower();
                if (!lowerProdName.Contains(keyWord.ToLower()))
                {
                    return false;
                }
            }

            return true;
        }

        private bool ValidProductRank(Product product, Dictionary<string, object> searchBy)
        {
            if (searchBy.ContainsKey("searchByProductRank"))
            {
                int minRank = (int)searchBy["searchByRank"];
                if (product.Rank < minRank)
                {
                    return false;
                }
            }
            return true;
        }

        private bool ValidPriceRange(Product product, Dictionary<string, object> searchBy)
        {
            if (searchBy.ContainsKey("searchByPriceRange"))
            {
                Tuple<double, double> priceRange = (Tuple<double, double>)searchBy["searchByPriceRange"];
                double minPrice = priceRange.Item1;
                double maxPrice = priceRange.Item2;
                if (product.Price > maxPrice || product.Price < minPrice)
                {
                    return false;
                }

            }
            return true;
        }

        private bool ValidProductCategory(Product product, Dictionary<string, object> searchBy)
        {

            if (searchBy.ContainsKey("searchByCategory"))
            {

                if (!product.Category.Equals(searchBy["searchByCategory"]))
                {
                    return false;
                }
            }

            return true;
        }

        private bool ValidStoreRank(Store store, Dictionary<string, object> searchBy)
        {

            if (searchBy.ContainsKey("SearchByStoreRank"))
            {
                int minRank = (int)searchBy["SearchByStoreRank"];
                if (store.Rank < minRank)
                {
                    return false;
                }
            }
            return true;
        }

        private bool ValidProductName(Product product, Dictionary<string, object> searchBy)
        {
            if (searchBy.ContainsKey("searchByName"))
            {
                string productOrigName = (string)searchBy["searchByName"];
                string productNameNoSpaces = product.Name.Replace(" ", "");
                string lowerProdName = productNameNoSpaces.ToLower();
                if (!lowerProdName.Equals(productOrigName.ToLower()))
                {
                    return false;
                }
            }
            return true;
        }



    }
}

