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
        double CalcDiscount(PurchaseBasket basket, PolicyValidator validator);
        string ToString();
    }

    public class CompundDiscount : DiscountPolicy
    {
        public List<DiscountPolicy> children;
        public int mergeType;
        public CompundDiscount(int mergeType, List<DiscountPolicy> children)
        {
            if (children == null)
                this.children = new List<DiscountPolicy>();
            else
                this.children = children;
            this.mergeType = mergeType;
        }

        public double CalcDiscount(PurchaseBasket basket, PolicyValidator validator)
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

        public int GetMergeType() 
        {
            return mergeType;
        }
        public override string ToString()
        {
            string ret = "(\n";
            ret += mergeType == 0 ? "XOR " : mergeType == 1 ? "OR " : mergeType == 2 ? "AND " : "UNKNOWN ";
            foreach (DiscountPolicy discount in children)
                ret += "\t\n" + discount.ToString() + ",";
            ret += "\n)";
            return ret;
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
        public double CalcDiscount(PurchaseBasket basket, PolicyValidator validator)
        {
            return 0;
        }

    }

 
    public class ConditionalProductDiscount : ConditionalDiscount
    {
        public int discountProdutId { get; set; }
        public ConditionalProductDiscount(int discountProdutId, PreCondition preCondition, double discount) : base(preCondition, discount)
        {
            this.discountProdutId = discountProdutId;
        }

        public override double CalcDiscount(PurchaseBasket basket, PolicyValidator validator)
        {
            double reduction = 0;
            if (PreCondition.IsFulfilled(basket, discountProdutId, validator))
            {
                int numProducts = basket.Products[discountProdutId];
                double price = basket.Store.GetProductDetails(discountProdutId).Item1.Price;
                reduction = numProducts * ((Discount / 100) * price);
            }
            return reduction;
        }
        public override string ToString()
        {
            //Inventory inv = new Inventory();
            //string productStr = inv.getProductDetails(discountProdutId).Item1.Name;
            Dictionary<int, string> dic = StoreManagment.Instance.GetAvilableRawDiscount();
            string preStr = dic[PreCondition.PreConditionNumber];
            return "[Conditional Product Discount: buy " + discountProdutId + " ," + preStr + " and get "+ Discount + "% off]";
        }
    }

    public class ConditionalBasketDiscount : ConditionalDiscount
    {
        public ConditionalBasketDiscount(PreCondition preCondition, double discount) : base(preCondition, discount)
        {
        }

        public override double CalcDiscount(PurchaseBasket basket, PolicyValidator validator)
        {
            if (PreCondition.IsFulfilled(basket, -1, validator))
                return (Discount / 100) * basket.GetBasketOrigPrice();
            return 0;
        }
        public override string ToString()
        {
            Dictionary<int, string> dic = StoreManagment.Instance.GetAvilableRawDiscount();
            string preStr = dic[PreCondition.PreConditionNumber];

            return "[Conditional Basket Discount:" + preStr + " and get " + Discount + "% off]";
        }

    }



    public class RevealdDiscount : DiscountPolicy
    {
        public double discount { get; set; }
        public int discountProdutId { get; set; }
        public RevealdDiscount(int discountProductId, double discount)
        {
            this.discountProdutId = discountProductId;
            this.discount = discount;
        }

        public double CalcDiscount(PurchaseBasket basket, PolicyValidator validator)
        {
            double reduction = 0;
            if (basket.Products.ContainsKey(discountProdutId))
            {
                int numProducts = basket.Products[discountProdutId];
                double price = basket.Store.GetProductDetails(discountProdutId).Item1.Price;
                reduction = numProducts * ((discount/100) * price);
            }
            return reduction; ;
        }
        public override string ToString()
        {
            return "[Reveald Discount: buy " + discountProdutId + " and get " + discount + "% off]";
        }
    }
}
