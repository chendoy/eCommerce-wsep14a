using eCommerce_14a.PurchaseComponent.DomainLayer;
using eCommerce_14a.StoreComponent;
using eCommerce_14a.StoreComponent.DomainLayer;
using eCommerce_14a.Utils;
using System;
using System.Collections.Generic;

namespace Server.StoreComponent.DomainLayer

{
    public class PreConditionValidator
    {
        private Dictionary<int, Func<Product, bool>> ProductsValidateFuncs;
        private Dictionary<int, Func<PurchaseBasket, bool>> BasketsValidateFuncs;
        public PreConditionValidator()
        {
            ProductsValidateFuncs = new Dictionary<int, Func<Product, bool>>();
            BasketsValidateFuncs = new Dictionary<int, Func<PurchaseBasket, bool>>();
        }

        public void AddProductValidateFunc(int preConditionNumber, Func<Product, bool> func)
        {
            if (!ProductsValidateFuncs.ContainsKey(preConditionNumber))
                ProductsValidateFuncs.Add(preConditionNumber, func);
        }

        public void RemoveProductValidateFunction(int preConditionNumber)
        {
            if (ProductsValidateFuncs.ContainsKey(preConditionNumber))
                ProductsValidateFuncs.Remove(preConditionNumber);
        }

        public void AddBasketValidateFunc(int preConditionNumber, Func<PurchaseBasket, bool> func)
        {
            if (!BasketsValidateFuncs.ContainsKey(preConditionNumber))
                BasketsValidateFuncs.Add(preConditionNumber, func);
        }

        public void RemoveBasketValidateFunction(int preConditionNumber)
        {
            if (BasketsValidateFuncs.ContainsKey(preConditionNumber))
                BasketsValidateFuncs.Remove(preConditionNumber);
        }

        public Dictionary<int, Func<Product, bool>>  ProductsValidiator
        {
            get { return ProductsValidateFuncs; }
        }

        public Dictionary<int, Func<PurchaseBasket, bool>> BasketsValidator
        {
            get { return BasketsValidateFuncs; }
        }
    }
}