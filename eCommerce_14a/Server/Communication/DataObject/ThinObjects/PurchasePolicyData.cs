using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Communication.DataObject.ThinObjects
{
    public class PurchasePolicyData
    {
        public int MergeType { get; set; }
        public List<PurchasePolicyData> PurchaseChildren { get; set; }
    }


    public class PurchasePolicy : PurchasePolicyData
    {
        public int PreCondition { get; set; }

        public PurchasePolicy()
        {
        }

        public PurchasePolicy(int preCondition)
        {
            PreCondition = preCondition;
        }
    }

    public class PurchasePolicySimple : PurchasePolicy
    {

        public PurchasePolicySimple(int preCondition) : base(preCondition)
        {
        }
    }

    public class PurchasePolicyProduct : PurchasePolicy
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

    public class PurchasePolicyBasket : PurchasePolicy
    {

        public PurchasePolicyBasket() : base()
        {
        }

        public PurchasePolicyBasket(int preCondition) : base(preCondition)
        {
        }

    }

    public class PurchasePolicySystem : PurchasePolicy
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

    public class PurchasePolicyUser : PurchasePolicy
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



