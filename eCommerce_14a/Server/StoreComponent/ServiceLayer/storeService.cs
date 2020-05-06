using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using eCommerce_14a.StoreComponent.DomainLayer;
using Server.StoreComponent.DomainLayer;

namespace eCommerce_14a.StoreComponent.ServiceLayer
{
    public class StoreService
    {
        StoreManagment storeManagment;
        Searcher searcher;
        public StoreService()
        {
            this.storeManagment = StoreManagment.Instance;
            this.searcher = new Searcher(storeManagment);

        }
        /// <req> https://github.com/chendoy/wsep_14a/wiki/Use-cases#use-case-overlook-details-about-stores-and-their-products-24 </req>
        public Dictionary<string, object> getStoreInfo(int storeId)
        {
            return storeManagment.getStoreInfo(storeId);
        }

        /// <req> https://github.com/chendoy/wsep_14a/wiki/Use-cases#use-case-inventory-management---add-product-411- </req>
        /// <req> https://github.com/chendoy/wsep_14a/wiki/Use-cases#use-case-store-manager--add-product-512- </req
        public Tuple<bool, string> appendProduct(int storeId, string userName, int productId, string productDetails, double productPrice, string productName, string productCategory, int amount)
        {
            return storeManagment.appendProduct(storeId, userName, productId, productDetails, productPrice, productName, productCategory, amount);
        }


        /// <req> https://github.com/chendoy/wsep_14a/wiki/Use-cases#use-case-inventory-management---edit-product-412- </req>
        /// <req> https://github.com/chendoy/wsep_14a/wiki/Use-cases#use-case-store-manager---edit-product-513- </req>
        public Tuple<bool, string> UpdateProduct(string userName, int storeId, int productId, string pDetails, double pPrice, string pName, string pCategory)
        {
            return storeManagment.UpdateProduct(userName, storeId, productId, pDetails, pPrice, pName, pCategory);
        }

        /// <req> https://github.com/chendoy/wsep_14a/wiki/Use-cases#use-case-inventory-management---remove-product-413- </req>
        /// <req> https://github.com/chendoy/wsep_14a/wiki/Use-cases#use-case-store-manager---remove-product-514- </req>
        public Tuple<bool, string> removeProduct(int storeId, string userName, int productId)
        {
            return storeManagment.removeProduct(storeId, userName, productId);
        }

        /// <req> https://github.com/chendoy/wsep_14a/wiki/Use-cases#use-case-inventory-management---increasedecrease-amount-414- </req>
        /// <req> https://github.com/chendoy/wsep_14a/wiki/Use-cases#use-case-store-manager---increasedecrease-amount-511 </req>
        public Tuple<bool, string> IncreaseProductAmount(int storeId, string userName, int productId, int amount)
        {
            return storeManagment.addProductAmount(storeId, userName, productId, amount);
        }

        /// <req> https://github.com/chendoy/wsep_14a/wiki/Use-cases#use-case-store-manager---increasedecrease-amount-511 </req>
        /// <req> https://github.com/chendoy/wsep_14a/wiki/Use-cases#use-case-inventory-management---increasedecrease-amount-414- </req>
        public Tuple<bool, string> decraseProduct(int storeId, string userName, int productId, int amount)
        {
            return storeManagment.decraseProductAmount(storeId, userName, productId, amount);
        }

        /// <req> https://github.com/chendoy/wsep_14a/wiki/Use-cases#use-case-subscription-buyer--open-store-32 </req>
        public Tuple<int, string> createStore(string userName, DiscountPolicy discuntPolicy, PurchasePolicy purchasePolicy, Validator validator)
        {
            return storeManagment.createStore(userName, discuntPolicy, purchasePolicy, validator);
        }


        public Tuple<bool, string> removeStore(string userName, int storeId)
        {
            return storeManagment.removeStore(userName, storeId);
        }


        public Tuple<bool, string> changeStoreStatus(string userName, int storeId, bool status)
        {
            return storeManagment.changeStoreStatus(userName, storeId, status);
        }

        /// <req> https://github.com/chendoy/wsep_14a/wiki/Use-cases#use-case-overlook-details-about-stores-and-their-products-24 </req>
        /// <req> https://github.com/chendoy/wsep_14a/wiki/Use-cases#use-case-search-for-products-25 </req>
        public Dictionary<int, List<Product>> SearchProducts(Dictionary<string, object> searchBy)
        {
            return searcher.SearchProducts(searchBy);
        }

        public List<Store> GetAllStores()
        {
            return storeManagment.GetAllStores();
        }

        //For Admin Uses
        public void cleanup()
        {
            storeManagment.cleanup();
        }

        public List<Store> GetAllStores()
        {
            return storeManagment.GetAllStores();
        }



    }
}
