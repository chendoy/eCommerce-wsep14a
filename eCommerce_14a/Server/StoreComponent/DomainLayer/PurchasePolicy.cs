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
    }
    public class CompundPurchasePolicy : PurchasePolicy
    {
        private List<PurchasePolicy> children;
        int mergeType;

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
        int policyProductId;
        public ProductPurchasePolicy(PreCondition pre, int policyProductId) : base(pre)
        {
            this.policyProductId = policyProductId;
        }

        public override bool IsEligiblePurchase(PurchaseBasket basket, Validator validator)
        {
            return PreCondition.IsFulfilled(basket, policyProductId, null, null, validator);
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
    }

    public class SystemPurchasePolicy : SimplePurchasePolicy
    {
        private Store store;
        public SystemPurchasePolicy(PreCondition pre,Store store) : base(pre)
        {
            this.store = store;
        }

        public override bool IsEligiblePurchase(PurchaseBasket basket, Validator validator)
        {
            return PreCondition.IsFulfilled(basket, -1, null, store, validator);
        }
    }

    public class UserPurchasePolicy : SimplePurchasePolicy
    {
        private User user;
        public UserPurchasePolicy(PreCondition pre, User user) : base(pre)
        {
            this.user = user;
        }

        public override bool IsEligiblePurchase(PurchaseBasket basket, Validator validator)
        {
            return PreCondition.IsFulfilled(basket, -1, user, null, validator);
        }
    }
}