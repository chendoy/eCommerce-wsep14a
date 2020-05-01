using eCommerce_14a.PurchaseComponent.DomainLayer;
using eCommerce_14a.StoreComponent;
using eCommerce_14a.StoreComponent.DomainLayer;
using eCommerce_14a.Utils;
using System;
using System.Collections.Generic;

namespace Server.StoreComponent.DomainLayer

{
    public class Validator
    {

        private Dictionary<int, Func<PurchaseBasket,int, bool>> discountValidateFunctions;

        public Validator(Dictionary<int, Func<PurchaseBasket, int, bool>> discountFunctions)
        {
            if (discountFunctions != null)
                this.discountValidateFunctions = discountFunctions;
            else
                discountValidateFunctions = new Dictionary<int, Func<PurchaseBasket,int, bool>>();
        }

  
        public void AddDiscountFunction(int preConditionNumber, Func<PurchaseBasket,int, bool> func)
        {
            if (!discountValidateFunctions.ContainsKey(preConditionNumber))
                discountValidateFunctions.Add(preConditionNumber, func);
        }

        public void RemoveDiscountFunction(int preConditionNumber)
        {
            if (discountValidateFunctions.ContainsKey(preConditionNumber))
                discountValidateFunctions.Remove(preConditionNumber);
        }


        public Dictionary<int, Func<PurchaseBasket, int, bool>> DiscountValidatorFuncs
        {
            get { return discountValidateFunctions; }
        }


    }
}