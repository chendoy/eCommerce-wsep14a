using eCommerce_14a.StoreComponent.DomainLayer;
using eCommerce_14a.PurchaseComponent.DomainLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eCommerce_14a.UserComponent.DomainLayer;

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

        public Validator Validator
        {
            get { return validator;}
        }

        public int PreConditionNumber
        {
            get { return preCondNumber; }
        }

        virtual
        public bool IsFulfilled(PurchaseBasket basket, int productId)
        {
            return false;
        }

        virtual
        public bool IsFulfilled(PurchaseBasket basket, int productId, User user, Store store)
        {
            return false;
        }

    }

    public class DiscountPreCondition : PreCondition
    {
        public DiscountPreCondition(int num, Validator validator) : base (num, validator)
        {

        }

        override
        public bool IsFulfilled(PurchaseBasket basket, int productId)
        {
            if (productId > 0)
                if (!basket.Products.ContainsKey(productId))
                    return false;
            return Validator.DiscountValidatorFuncs[PreConditionNumber].Invoke(basket, productId);
        }

    }

    public class PurchasePreCondition : PreCondition
    {
        public PurchasePreCondition(int num, Validator validator): base (num, validator)
        {

        }

        override
        public bool IsFulfilled(PurchaseBasket basket, int productId, User user, Store store)
        {
            if (productId > 0)
                if (!basket.Products.ContainsKey(productId))
                    return false;
            return Validator.PurchaseValidatorFuncs[PreConditionNumber].Invoke(basket, productId, user, store);
        }
    }






 




}
