using eCommerce_14a.PurchaseComponent.DomainLayer;
using eCommerce_14a.UserComponent.DomainLayer;
using eCommerce_14a.Utils;
using Server.StoreComponent.DomainLayer;
using System.Collections.Generic;


namespace eCommerce_14a.StoreComponent.DomainLayer
{  
    public interface PurchasePolicy
    {
        bool IsEligiblePurchase(PurchaseBasket basket, PolicyValidator validator);
        string Describe(int depth);
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

        public bool IsEligiblePurchase(PurchaseBasket basket, PolicyValidator validator)
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

        public string Describe(int depth)
        {
            string pad = "";
            for (int i = 0; i < depth; i++) { pad += "    "; }
            string ret = pad + "(";
            ret += "    ";
            ret += mergeType == 0 ? "XOR " : mergeType == 1 ? "OR " : mergeType == 2 ? "AND " : "UNKNOWN ";
            foreach (PurchasePolicy policy in children)
                ret += "\n" + policy.Describe(depth + 1) + ",";
            ret += "\n" + pad + ")";
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
        public bool IsEligiblePurchase(PurchaseBasket basket, PolicyValidator validator);
        public abstract string Describe(int depth);

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

        public override bool IsEligiblePurchase(PurchaseBasket basket, PolicyValidator validator)
        {
            return PreCondition.IsFulfilled(basket, policyProductId, null, -1, validator);
        }

        public override string Describe(int depth)
        {
            //Inv6entory inv = new Inventory();
            //string productStr = inv.getProductDetails(policyProductId).Item1.Name;
            Dictionary<int, string> dic = StoreManagment.Instance.GetAvilableRawDiscount();
            string preStr = dic[PreCondition.PreConditionNumber];
            string pad = "";
            for (int i = 0; i < depth; i++) { pad += "    "; }
            return pad + "[Product Purchase Policy: " + policyProductId + " - " + preStr + " ]";
        }
    }

    public class BasketPurchasePolicy : SimplePurchasePolicy
    {

        public BasketPurchasePolicy(PreCondition pre) : base(pre)
        {

        }

        public override bool IsEligiblePurchase(PurchaseBasket basket, PolicyValidator validator)
        {
            return PreCondition.IsFulfilled(basket, -1, null, -1, validator);
        }

        public override string Describe(int depth)
        {
            Dictionary<int, string> dic = StoreManagment.Instance.GetAvilableRawPurchasePolicy();
            string preStr = dic[PreCondition.PreConditionNumber];
            string pad = "";
            for (int i = 0; i < depth; i++) { pad += "    "; }
            return pad + "[Basket Purchase Policy: " + preStr + "]";
        }
    }

    public class SystemPurchasePolicy : SimplePurchasePolicy
    {
        public  int  StoreId { get; set; }
        public SystemPurchasePolicy(PreCondition pre,int storeId) : base(pre)
        {
            StoreId = storeId;
        }

        public override bool IsEligiblePurchase(PurchaseBasket basket, PolicyValidator validator)
        {
            return PreCondition.IsFulfilled(basket, -1, null, StoreId, validator);
        }

        public override string Describe(int depth)
        {
            Dictionary<int, string> dic = StoreManagment.Instance.GetAvilableRawPurchasePolicy();
            string preStr = dic[PreCondition.PreConditionNumber];
            string pad = "";
            for (int i = 0; i < depth; i++) { pad += "    "; }
            return pad + "[System Purchase Policy: " + preStr + " in store #" + StoreId + "]";
        }
    }

    public class UserPurchasePolicy : SimplePurchasePolicy
    {
        public UserPurchasePolicy(PreCondition pre) : base(pre)
        {
        }

        public override bool IsEligiblePurchase(PurchaseBasket basket, PolicyValidator validator)
        {
            return PreCondition.IsFulfilled(basket, -1, basket.User, -1, validator);
        }

        public override string Describe(int depth)
        {
            Dictionary<int, string> dic = StoreManagment.Instance.GetAvilableRawPurchasePolicy();
            string preStr = dic[PreCondition.PreConditionNumber];
            string pad = "";
            for (int i = 0; i < depth; i++) { pad += "    "; }
            return pad + "[User Purchase Policy: " + preStr + "]";
        }
    }
}