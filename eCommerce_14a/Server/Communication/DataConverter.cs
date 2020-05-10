using eCommerce_14a.PurchaseComponent.DomainLayer;
using eCommerce_14a.StoreComponent.DomainLayer;
using eCommerce_14a.UserComponent.DomainLayer;
using System.Linq;
using Server.Communication.DataObject.ThinObjects;
using System.Collections.Generic;
using System;

namespace Server.Communication
{
    class DataConverter
    {
        public DataConverter() { }
        public ProductData ToProductData(Product prod) 
        {
            return new ProductData(prod.ProductID, prod.Name,prod.Category,prod.Details,prod.Price,prod.ImgUrl);
        }

        public CartData ToCartData(Cart cart) 
        {
            return new CartData(cart.user, ToPurchaseBasketDataList(cart.baskets.Values.ToList()));
        }

        public PurchaseData ToPurchaseData(Purchase purchase) 
        {
            return new PurchaseData(purchase.User, ToCartData(purchase.UserCart));
        }

        public StoreData ToStoreData(Store store) 
        {
            return new StoreData(store.Id, ToUserNameList(store.owners), ToUserNameList(store.managers), ToInventoryData(store.Inventory));
        }

        //public UserData ToUserData(User user)
        //{
        //    return new UserData(user.getUserName());
        //}

        public PurchaseBasketData ToPurchaseBasketData(PurchaseBasket pBasket)
        {
            return new PurchaseBasketData(ToStoreData(pBasket.Store), pBasket.user, pBasket.Price, pBasket.PurchaseTime, pBasket.Products);
        }

        public InventoryData ToInventoryData(Inventory inv)
        {
            List<Tuple<Product,int>> prodList = inv.Inv.Values.ToList();
            List<Tuple<ProductData, int>> retList = new List<Tuple<ProductData, int>>();
            foreach (Tuple<Product, int> tup in prodList) 
            {
                retList.Add(new Tuple<ProductData, int>(ToProductData(tup.Item1), tup.Item2));
            }
            return new InventoryData(retList);
        }

        public List<StoreData> ToProductDataList(List<Store> stores)
        {
            List<StoreData> retList = new List<StoreData>();
            foreach (Store store in stores)
            {
                retList.Add(ToStoreData(store));
            }
            return retList;
        }

        public List<ProductData> ToProductDataList(List<Product> products) 
        {
            List<ProductData> retList = new List<ProductData>();
            foreach (Product prod in products)
            {
                retList.Add(ToProductData(prod));
            }
            return retList;
        }

        public List<string> ToUserNameList(List<User> users)
        {
            List<string> retlist = new List<string>();
            foreach (User user in users)
            {
                retlist.Add(user.getUserName());
            }
            return retlist;
        }

        public List<PurchaseBasketData> ToPurchaseBasketDataList(List<PurchaseBasket> pBaskets)
        {
            List<PurchaseBasketData> retList = new List<PurchaseBasketData>();
            foreach (PurchaseBasket pBasket in pBaskets)
            {
                retList.Add(ToPurchaseBasketData(pBasket));
            }
            return retList;
        }

        public List<PurchaseData> ToPurchaseDataList(List<Purchase> purchases)
        {
            List<PurchaseData> retList = new List<PurchaseData>();
            foreach (Purchase purchase in purchases)
            {
                retList.Add(ToPurchaseData(purchase));
            }
            return retList;
        }
    }
}
