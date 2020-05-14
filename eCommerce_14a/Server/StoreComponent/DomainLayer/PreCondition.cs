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
        public PreCondition(int num)
        {
            this.preCondNumber = num;
        }


        public int PreConditionNumber
        {
            get { return preCondNumber; }
        }

        virtual
        public bool IsFulfilled(PurchaseBasket basket, int productId, Validator validator)
        {
            return false;
        }

        virtual
        public bool IsFulfilled(PurchaseBasket basket, int productId, User user, Store store, Validator validator)
        {
            return false;
        }

    }

    public class DiscountPreCondition : PreCondition
    {
        public DiscountPreCondition(int num) : base (num)
        {

        }

        override
        public bool IsFulfilled(PurchaseBasket basket, int productId, Validator validator)
        {
            return validator.DiscountValidatorFuncs[PreConditionNumber].Invoke(basket, productId);
        }

    }

    public class PurchasePreCondition : PreCondition
    {
        public PurchasePreCondition(int num): base (num)
        {

        }

        override
        public bool IsFulfilled(PurchaseBasket basket, int productId, User user, Store store, Validator validator)
        {

            return validator.PurchaseValidatorFuncs[PreConditionNumber].Invoke(basket, productId, user, store);
        }
    }






 




}
