using Server.StoreComponent.DomainLayer;
using eCommerce_14a.PurchaseComponent.DomainLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce_14a.StoreComponent.DomainLayer
{

    public interface DiscountRule
    {
        double calcDiscount(Product product);
        double calcDiscount(PurchaseBasket basket);
    }

    public class CompundDiscount : DiscountRule
    {
        private List<DiscountRule> children;

        public CompundDiscount()
        {
            children = new List<DiscountRule>();
        }

        public void add(DiscountRule discountRule)
        {
            children.Add(discountRule);
        }
        public void remove(DiscountRule discount)
        {
            children.Remove(discount);
        }

        public List<DiscountRule> getChildren()
        {
            return children;
        }

        public double calcDiscount(Product product)
        {
            throw new NotImplementedException();
        }

        public double calcDiscount(PurchaseBasket basket)
        {
            throw new NotImplementedException();
        }
    }

    abstract
    public class ConditionalDiscount : DiscountRule
    {
        private PreCondition preCondtion;
        private double discount;
        public ConditionalDiscount(PreCondition preCondtion, double discount)
        {
            this.preCondtion = preCondtion;
            this.discount = discount;
        }

        public PreCondition preConditon
        {
            get { return preConditon; }
        }

        public double Discount
        {
            get { return discount; }
        }

        virtual
        public double calcDiscount(Product product)
        {
            return 0;
        }

        virtual
        public double calcDiscount(PurchaseBasket basket)
        {
            return 0;
        }
    }

    public class ConditionalProductDiscount : ConditionalDiscount
    {
        public ConditionalProductDiscount(PreCondition preCondition, double discount): base(preCondition, discount)
        {
        }

        
        public override double calcDiscount(Product p)
        {

            if (base.preConditon.IsFulfilled(p))
                return base.Discount;
            else
                return 0;
        }


    }

    public class ConditionalBasketDiscount : ConditionalDiscount
    {
        public ConditionalBasketDiscount(PreCondition preCondition, double discount) : base(preCondition, discount)
        {
        }

        public override double calcDiscount(PurchaseBasket basket)
        {
            if (base.preConditon.IsFulfilled(basket))
                return base.Discount;
            else
                return 0;
        }


    }



    public class RevealdDiscount : DiscountRule
    {
        private double discount;
        private Product discount_product;
        public RevealdDiscount(Product product, double discount)
        {
            this.discount_product = product;
            this.discount = discount;
        }

        public double calcDiscount(Product product)
        {
            if (product.ProductID == discount_product.ProductID)
                return discount;
            else
                return 0;
        }

        public double calcDiscount(PurchaseBasket basket)
        {
            return 0;
        }
    }
}
