using eCommerce_14a.StoreComponent.DomainLayer;
using eCommerce_14a.PurchaseComponent.DomainLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.StoreComponent.DomainLayer
{
    public  class PreCondition
    {
        int preCondNumber;
        Validator validator;
        public PreCondition(int num, Validator validator)
        {
            this.preCondNumber = num;
            this.validator = validator;
        }

        public  bool IsFulfilled(PurchaseBasket basket, int productId)
        {
            if (productId > 0)
                if (!basket.Products.ContainsKey(productId))
                    return false;
            return validator.DiscountValidatorFuncs[preCondNumber].Invoke(basket, productId);
        }

    }






 




}
