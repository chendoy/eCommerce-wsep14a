using eCommerce_14a.StoreComponent.DomainLayer;
using eCommerce_14a.PurchaseComponent.DomainLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.StoreComponent.DomainLayer
{
    public abstract class PreCondition
    {
        int preCondNumber;
        PreConditionValidator validator;
        public PreCondition(int num, PreConditionValidator validator)
        {
            this.preCondNumber = num;
            this.validator = validator;
        }

        public int PreCondNumber
        {
            get { return preCondNumber; }
        }
        public PreConditionValidator Validator
        {
            get { return validator; }
        }

        virtual
        public bool IsFulfilled(Product p)
        {
            return false;
        }

        virtual
        public bool IsFulfilled(PurchaseBasket basket)
        {
            return false;
        }


    }

    public class ProductPreCondition : PreCondition
    {
        public ProductPreCondition(int preCondNumber, PreConditionValidator validator) : base(preCondNumber, validator)
        {
        }

        public override bool IsFulfilled(Product p)
        {
            return base.Validator.ProductsValidiator[base.PreCondNumber].Invoke(p);
        }
    }

    public class BasketPreCondition : PreCondition
    {
        public BasketPreCondition(int preCondNumber, PreConditionValidator validator) : base(preCondNumber, validator)
        {
        }

        public override bool IsFulfilled(PurchaseBasket basket)
        {
            return base.Validator.BasketsValidator[base.PreCondNumber].Invoke(basket);
        }
    }


}
