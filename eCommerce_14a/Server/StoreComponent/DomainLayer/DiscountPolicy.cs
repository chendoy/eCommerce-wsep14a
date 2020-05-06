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
    public interface DiscountPolicy
    {
        double CalcDiscount(PurchaseBasket basket, Validator validator);
    }

    public class CompundDiscount : DiscountPolicy
    {
        private List<DiscountPolicy> children;
        int mergeType;
        public CompundDiscount(int mergeType, List<DiscountPolicy> children)
        {
            if (children == null)
                this.children = new List<DiscountPolicy>();
            else
                this.children = children;
            this.mergeType = mergeType;
        }

        public double CalcDiscount(PurchaseBasket basket, Validator validator)
        {
            if (mergeType == CommonStr.DiscountMergeTypes.OR)
            {
                double sum_discounts = 0;
                foreach (DiscountPolicy child in children)
                    sum_discounts += child.CalcDiscount(basket, validator);
                return sum_discounts;
            } 
            else if (mergeType == CommonStr.DiscountMergeTypes.XOR)
            {
                double maxDiscount = 0;
                foreach(DiscountPolicy child in children)
                {
                    double discount = child.CalcDiscount(basket, validator);
                    if (discount > maxDiscount)
                        maxDiscount = discount;
                }
                return maxDiscount;
            }
            else if (mergeType == CommonStr.DiscountMergeTypes.AND)
            {
                return 0;
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
        private  PreCondition preCondtion;
        private double discount;
        public ConditionalDiscount(PreCondition preCondtion, double discount)
        {
            this.preCondtion = preCondtion;
            this.discount = discount;
        }

       
        public PreCondition PreCondition
        {
            get { return preCondtion; }
        }

        public double Discount
        {
            get { return discount; }
        }

       
        virtual
        public double CalcDiscount(PurchaseBasket basket, Validator validator)
        {
            return 0;
        }

    }

 
    public class ConditionalProductDiscount : ConditionalDiscount
    {
        int discountProdutId;
        public ConditionalProductDiscount(int discountProdutId, PreCondition preCondition, double discount) : base(preCondition, discount)
        {
            this.discountProdutId = discountProdutId;
        }

        public override double CalcDiscount(PurchaseBasket basket, Validator validator)
        {
            double reduction = 0;
            if (PreCondition.IsFulfilled(basket, discountProdutId, validator))
            {
                int numProducts = basket.Products[discountProdutId];
                double price = basket.Store.getProductDetails(discountProdutId).Item1.Price;
                reduction = numProducts * ((Discount / 100) * price);
            }
            return reduction;
        }
    }

    public class ConditionalBasketDiscount : ConditionalDiscount
    {
        public ConditionalBasketDiscount(PreCondition preCondition, double discount) : base(preCondition, discount)
        {
        }

        public override double CalcDiscount(PurchaseBasket basket, Validator validator)
        {
            if (PreCondition.IsFulfilled(basket, -1, validator))
                return (Discount / 100) * basket.GetBasketPrice();
            return 0;
        }

    }



    public class RevealdDiscount : DiscountPolicy
    {
        private double discount;
        private int discountProdutId;
        public RevealdDiscount(int discountProductId, double discount)
        {
            this.discountProdutId = discountProductId;
            this.discount = discount;
        }

        public double CalcDiscount(PurchaseBasket basket, Validator validator)
        {
            double reduction = 0;
            if (basket.Products.ContainsKey(discountProdutId))
            {
                int numProducts = basket.Products[discountProdutId];
                double price = basket.Store.getProductDetails(discountProdutId).Item1.Price;
                reduction = numProducts * ((discount/100) * price);
            }
            return reduction; ;
        }

    }
}
