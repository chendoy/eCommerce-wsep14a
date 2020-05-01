using Server.StoreComponent.DomainLayer;
using eCommerce_14a.PurchaseComponent.DomainLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eCommerce_14a.Utils;
using System.Globalization;

namespace eCommerce_14a.StoreComponent.DomainLayer
{
    enum MergeTypes : int
    {
        XOR = 0,
        AND = 1
    }
    public interface DiscountPolicy
    {
        double CalcDiscount(PurchaseBasket basket);
    }

    public class CompundDiscount : DiscountPolicy
    {
        private List<DiscountPolicy> children;
        int mergeType;
        public CompundDiscount(int mergeType)
        {
            List<DiscountPolicy> children = new List<DiscountPolicy>();
            this.mergeType = mergeType;
        }

        public double CalcDiscount(PurchaseBasket basket)
        {
            if (mergeType == (int)MergeTypes.AND)
            {
                double sum_discounts = 0;
                foreach (DiscountPolicy child in children)
                    sum_discounts += child.CalcDiscount(basket);
                return sum_discounts;
            } 
            else if (mergeType == (int)MergeTypes.XOR)
            {
                double maxDiscount = 0;
                foreach(DiscountPolicy child in children)
                {
                    double discount = child.CalcDiscount(basket);
                    if (discount > maxDiscount)
                        maxDiscount = discount;
                }
                return maxDiscount;
            }
            else
            {
                return 0;
            }
        }

        public void add(DiscountPolicy discountRule)
        {
            children.Add(discountRule);
        }
        public void remove(DiscountPolicy discount)
        {
            children.Remove(discount);
        }

        public List<DiscountPolicy> getChildren()
        {
            return children;
        }

    }

    abstract
    public class ConditionalDiscount : DiscountPolicy
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
        public double CalcDiscount(PurchaseBasket basket)
        {
            return 0;
        }

        public PreCondition GetPreCondition()
        {
            return preConditon;
        }
    }

 
    public class ConditionalProductDiscount : ConditionalDiscount
    {
        Product discountProdut;
        public ConditionalProductDiscount(PreCondition preCondition, double discount, Product discountProdut) : base(preCondition, discount)
        {
            this.discountProdut = discountProdut;
        }

        public override double CalcDiscount(PurchaseBasket basket)
        {
            double reduction = 0;
            if (base.preConditon.IsFulfilled(basket))
            {
                int numProducts = basket.Products[discountProdut.ProductID];
                reduction = numProducts * ((Discount / 100) * discountProdut.Price);
            }
            return reduction;
        }
    }

    public class ConditionalBasketDiscount : ConditionalDiscount
    {
        public ConditionalBasketDiscount(PreCondition preCondition, double discount) : base(preCondition, discount)
        {
        }

        public override double CalcDiscount(PurchaseBasket basket)
        {
            if (base.preConditon.IsFulfilled(basket))
                return (Discount / 100) * basket.GetBasketPrice();
            return 0;
        }

    }



    public class RevealdDiscount : DiscountPolicy
    {
        private double discount;
        private Product discountProdut;
        public RevealdDiscount(Product discountProdut, double discount)
        {
            this.discountProdut = discountProdut;
            this.discount = discount;
        }

        public double CalcDiscount(PurchaseBasket basket)
        {
            double reduction = 0;
            if (basket.Products.ContainsKey(discountProdut.ProductID))
            {
                int numProducts = basket.Products[discountProdut.ProductID];
                reduction = numProducts * ((discount/100) * discountProdut.Price);
            }
            return reduction; ;
        }

    }
}
