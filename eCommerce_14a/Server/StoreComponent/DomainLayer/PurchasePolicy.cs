using eCommerce_14a.PurchaseComponent.DomainLayer;
using eCommerce_14a.UserComponent.DomainLayer;
using eCommerce_14a.Utils;
using Server.StoreComponent.DomainLayer;
using System.Collections.Generic;

namespace eCommerce_14a.StoreComponent.DomainLayer
{  
    public interface PurchasePolicy
    {
        bool IsEligiblePurchase(PurchaseBasket basket, Validator validator);
        string ToString();
    }
    public class CompundPurchasePolicy : PurchasePolicy
    {
        private List<PurchasePolicy> children;
        public int mergeType { get; set; }

        public CompundPurchasePolicy(int mergeType, List<PurchasePolicy> children)
        {
            if (children != null)
                this.children = children;
            else
                this.children = new List<PurchasePolicy>();
            this.mergeType = mergeType;
        }

        public bool IsEligiblePurchase(PurchaseBasket basket, Validator validator)
        {
            if (mergeType == CommonStr.PurchaseMergeTypes.AND)
            {
                foreach (PurchasePolicy child in children)
                    if (!child.IsEligiblePurchase(basket, validator))
                        return false;
                return true;
            }
            else if (mergeType == CommonStr.PurchaseMergeTypes.OR)
            {
                foreach (PurchasePolicy child in children)
                    if (child.IsEligiblePurchase(basket, validator))
                        return true;
                return false;
            }
            else if (mergeType == CommonStr.PurchaseMergeTypes.XOR)
            {
                // assuming xor accept only 2 inputs at most!
                if (children.Count == 0)
                    return true;
                if (children.Count > 2)
                    return false;
                if (children.Count == 1)
                    return children[0].IsEligiblePurchase(basket, validator);
                if(children.Count == 2)
                {
                    bool firstRes = children[0].IsEligiblePurchase(basket, validator);
                    bool secondRes = children[1].IsEligiblePurchase(basket, validator);
                    return firstRes & !secondRes || secondRes & !firstRes;
                }
                return false;   
            }
            else
            {
                return false;
            }
        }
        public void add(PurchasePolicy purchasePolicy)
        {
            children.Add(purchasePolicy);
        }
        public void remove(PurchasePolicy purchasePolicy)
        {
            children.Remove(purchasePolicy);
        }

        public List<PurchasePolicy> getChildren()
        {
            return children;
        }

        public override string ToString()
        {
            string ret = "(";
            ret += mergeType == 0 ? "XOR " : mergeType == 1 ? "OR " : mergeType == 2 ? "AND " : "UNKNOWN ";
            foreach (DiscountPolicy discount in children)
                ret += discount.ToString() + " ";
            ret += ")";
            return ret;
        }
    }

    abstract 
    public class SimplePurchasePolicy : PurchasePolicy
    {
        private PreCondition preCondition;
        public SimplePurchasePolicy(PreCondition preCondition)
        {
            this.preCondition = preCondition;
        }

        abstract
        public bool IsEligiblePurchase(PurchaseBasket basket, Validator validator);

        public PreCondition PreCondition
        {
            get { return preCondition; }
        }
    }

    public class ProductPurchasePolicy : SimplePurchasePolicy
    {
        public int policyProductId { get; set; }
        public ProductPurchasePolicy(PreCondition pre, int policyProductId) : base(pre)
        {
            this.policyProductId = policyProductId;
        }

        public override bool IsEligiblePurchase(PurchaseBasket basket, Validator validator)
        {
            return PreCondition.IsFulfilled(basket, policyProductId, null, null, validator);
        }

        public override string ToString()
        {
            Inventory inv = new Inventory();
            string productStr = inv.getProductDetails(policyProductId).Item1.Name;
            StoreManagment sm = new StoreManagment();
            Dictionary<int, string> dic = sm.GetAvilableRawDiscount();
            string preStr = dic[PreCondition.PreConditionNumber];
            return "[Product Purchase Policy: " + productStr + " - " + preStr + " ]";
        }
    }

    public class BasketPurchasePolicy : SimplePurchasePolicy
    {

        public BasketPurchasePolicy(PreCondition pre) : base(pre)
        {

        }

        public override bool IsEligiblePurchase(PurchaseBasket basket, Validator validator)
        {
            return PreCondition.IsFulfilled(basket, -1, null, null, validator);
        }

        public override string ToString()
        {
            StoreManagment sm = new StoreManagment();
            Dictionary<int, string> dic = sm.GetAvilableRawDiscount();
            string preStr = dic[PreCondition.PreConditionNumber];
            return "[Basket Purchase Policy: " + preStr + "]";
        }
    }

    public class SystemPurchasePolicy : SimplePurchasePolicy
    {
        public  Store store { get; set; }
        public SystemPurchasePolicy(PreCondition pre,Store store) : base(pre)
        {
            this.store = store;
        }

        public override bool IsEligiblePurchase(PurchaseBasket basket, Validator validator)
        {
            return PreCondition.IsFulfilled(basket, -1, null, store, validator);
        }

        public override string ToString()
        {
            StoreManagment sm = new StoreManagment();
            Dictionary<int, string> dic = sm.GetAvilableRawDiscount();
            string preStr = dic[PreCondition.PreConditionNumber];
            return "[SystemPurchasePolicy: " + preStr + " in store #" + store.Id + "]";
        }
    }

    public class UserPurchasePolicy : SimplePurchasePolicy
    {
        public User user { get; set; }
        public UserPurchasePolicy(PreCondition pre, User user) : base(pre)
        {
            this.user = user;
        }

        public override bool IsEligiblePurchase(PurchaseBasket basket, Validator validator)
        {
            return PreCondition.IsFulfilled(basket, -1, user, null, validator);
        }

        public override string ToString()
        {
            StoreManagment sm = new StoreManagment();
            Dictionary<int, string> dic = sm.GetAvilableRawDiscount();
            string preStr = dic[PreCondition.PreConditionNumber];
            return "[User Purchase Policy: " + preStr + " on user \"" + user.getUserName() + "\"]";
        }
    }
}