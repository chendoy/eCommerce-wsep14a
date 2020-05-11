using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Communication.DataObject.ThinObjects
{
    public class PurchasePolicyData
    {
    }

    public class CompoundPurchasePolicyData : PurchasePolicyData
    {
        public CompoundPurchasePolicyData(int mergeType, List<PurchasePolicyData> purchaseChildren)
        {
            MergeType = mergeType;
            PurchaseChildren = purchaseChildren;
        }

        public int MergeType { get; set; }
        public List<PurchasePolicyData> PurchaseChildren { get; set; }
    }


    public class ThinPurchasePolicy : PurchasePolicyData
    {
        public int PreCondition { get; set; }

        public ThinPurchasePolicy()
        {
        }

        public ThinPurchasePolicy(int preCondition)
        {
            PreCondition = preCondition;
        }
    }

    public class PurchasePolicySimple : ThinPurchasePolicy
    {

        public PurchasePolicySimple(int preCondition) : base(preCondition)
        {
        }
    }

    public class PurchasePolicyProduct : ThinPurchasePolicy
    {
        public int ProductId { get; set; }

        public PurchasePolicyProduct() : base()
        {
        }

        public PurchasePolicyProduct(int preCondition, int productId) : base(preCondition)
        {
            ProductId = productId;
        }

    }

    public class PurchasePolicyBasket : ThinPurchasePolicy
    {

        public PurchasePolicyBasket() : base()
        {
        }

        public PurchasePolicyBasket(int preCondition) : base(preCondition)
        {
        }

    }

    public class PurchasePolicySystem : ThinPurchasePolicy
    {
        public int StoreId { get; set; }
        public PurchasePolicySystem() : base()
        {
        }

        public PurchasePolicySystem(int preCondition, int storeId) : base(preCondition)
        {
            StoreId = storeId;
        }

    }

    public class PurchasePolicyUser : ThinPurchasePolicy
    {
        public string UserName { get; set; }
        public PurchasePolicyUser() : base()
        {
        }

        public PurchasePolicyUser(int preCondition, string userName) : base(preCondition)
        {
            UserName = userName;
        }

    }









}



