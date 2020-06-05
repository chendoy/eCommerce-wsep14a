using Server.StoreComponent.DomainLayer;
using eCommerce_14a.PurchaseComponent.DomainLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eCommerce_14a.Utils;
using System.Globalization;
using System.Numerics;

namespace eCommerce_14a.StoreComponent.DomainLayer
{
    public interface DiscountPolicy
    {
        double CalcDiscount(PurchaseBasket basket);
        string Describe(int depth);
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

        public double CalcDiscount(PurchaseBasket basket)
        {
            if (mergeType == CommonStr.DiscountMergeTypes.OR)
            {
                double sum_discounts = 0;
                foreach (DiscountPolicy child in children)
                    sum_discounts += child.CalcDiscount(basket);
                return sum_discounts;
            } 
            else if (mergeType == CommonStr.DiscountMergeTypes.XOR)
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
        public string Describe(int depth)
        {
            string pad = "";
            for (int i = 0; i < depth; i++) { pad += "    "; }
            string ret = pad + "(";
            ret += mergeType == 0 ? "XOR " : mergeType == 1 ? "OR " : mergeType == 2 ? "AND " : "UNKNOWN ";
            foreach (DiscountPolicy discount in children)
                ret += "\n" + discount.Describe(depth + 1) + ",";
            ret += "\n" + pad + ")";
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
        public double CalcDiscount(PurchaseBasket basket)
        {
            return 0;
        }

        public abstract string Describe(int depth);
    }

 
    public class ConditionalProductDiscount : ConditionalDiscount
    {
        public int discountProdutId { get; set; }
        public int MinUnits { get; set; }
        public ConditionalProductDiscount(int discountProdutId, PreCondition preCondition, double discount) : base(preCondition, discount)
        {
            this.discountProdutId = discountProdutId;
            MinUnits = int.MaxValue;
        }
        public ConditionalProductDiscount(PreCondition preCondition, double discount, int minUnits) : base(preCondition, discount)
        {
            this.discountProdutId = -1;
            MinUnits = minUnits;
        }

        public override double CalcDiscount(PurchaseBasket basket)
        {
            if(PreCondition.preCondNumber == CommonStr.DiscountPreConditions.ProductPriceAboveX)
            {
                double reduction = 0;
                foreach(int pid in basket.Products.Keys)
                {
                    if(PreCondition.IsFulfilledProductPriceAboveXDiscount(basket, pid, MinUnits))
                    {
                        reduction += (Discount/100) *  basket.Store.GetProductDetails(pid).Item1.Price;
                    }
                }
                return reduction;
            }
            else if(PreCondition.preCondNumber == CommonStr.DiscountPreConditions.NumUnitsOfProductAboveX)
            {
                double reduction = 0;
                if(PreCondition.IsFufillledMinProductUnitDiscount(basket, discountProdutId, MinUnits))
                {
                    int amount = basket.Products[discountProdutId];
                    reduction = (Discount / 100) * basket.Store.GetProductDetails(discountProdutId).Item1.Price * amount;
                }
                return reduction;
            }
            else
            {
                return 0;
            }
            
        }
        public override string Describe(int depth)
        {
            //Inventory inv = new Inventory();
            //string productStr = inv.getProductDetails(discountProdutId).Item1.Name;
            Dictionary<int, string> dic = StoreManagment.Instance.GetAvilableRawDiscount();
            string preStr = dic[PreCondition.PreConditionNumber];
            string pad = "";
            for (int i = 0; i < depth; i++) { pad += "    "; }
            return pad + "[Conditional Product Discount: buy product #" + discountProdutId + " ," + preStr + " and get "+ Discount + "% off]";
        }
    }

    public class ConditionalBasketDiscount : ConditionalDiscount
    {
        public double MinPrice { set; get; }
        public int MinUnits { set; get; }
        public ConditionalBasketDiscount(PreCondition preCondition, double discount, double minPrice) : base(preCondition, discount)
        {
            MinPrice = minPrice;
            MinUnits= int.MaxValue;
        }

        public ConditionalBasketDiscount(PreCondition preCondition, double discount, int minUnits): base(preCondition, discount)
        {
            MinUnits = minUnits;
            MinPrice = int.MaxValue;
        }

        public override double CalcDiscount(PurchaseBasket basket)
        {
            if (PreCondition.PreConditionNumber == CommonStr.DiscountPreConditions.BasketPriceAboveX)
            {
                if (PreCondition.IsFulfilledMinBasketPriceDiscount(basket, MinPrice))
                {
                    return (Discount / 100) * basket.GetBasketOrigPrice();
                }
                else
                {
                    return 0;
                }
            }
            else if(PreCondition.preCondNumber == CommonStr.DiscountPreConditions.NumUnitsInBasketAboveX)
            {
                if(PreCondition.IsFulfilledMinUnitsAtBasketDiscount(basket, MinUnits))
                {
                    return (Discount / 100) * basket.GetBasketOrigPrice();

                }
                else
                {
                    return 0;
                }
            }
            else
            {
                return 0;
            }
        
        }
        public override string Describe(int depth)
        {
            Dictionary<int, string> dic = StoreManagment.Instance.GetAvilableRawDiscount();
            string preStr = dic[PreCondition.PreConditionNumber];
            string pad = "";
            for (int i = 0; i < depth; i++) { pad += "    "; }
            return pad + "[Conditional Basket Discount:" + preStr + " and get " + Discount + "% off]";
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

        public double CalcDiscount(PurchaseBasket basket)
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
        public string Describe(int depth)
        {
            string pad = "";
            for (int i = 0; i < depth; i++) { pad += "    "; }
            return pad + "[Reveald Discount: buy product #" + discountProdutId + " and get " + discount + "% off]";
        }
    }

   


}
